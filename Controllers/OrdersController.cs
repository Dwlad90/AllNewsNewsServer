using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using SalesAudit.Data.Constants;
using Tenli.Server.Data;
using Tenli.Server.Data.Constants;
using Tenli.Server.Data.DTOs.Order;
using Tenli.Server.Data.DTOs.OrderStatus;
using Tenli.Server.Data.Models;
using Tenli.Server.Services;

namespace Tenli.Server.Controllers {
  [Route ("api/v1/[controller]")]
  public class OrdersController : Controller {
    private readonly IMapper _mapper;
    private readonly OrderService _orderService;
    private readonly OrderStatusService _orderStatusService;
    private readonly ApplicationUserService _applicationUserService;
    private readonly CultureService _cultureService;
    private readonly ProductService _productService;

    public OrdersController (
      IMapper mapper,
      OrderService orderService,
      ApplicationUserService applicationUserService,
      CultureService cultureService,
      OrderStatusService orderStatusService,
      ProductService productService) {
      _mapper = mapper;
      _orderService = orderService;
      _applicationUserService = applicationUserService;
      _cultureService = cultureService;
      _orderStatusService = orderStatusService;
      _productService = productService;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType (typeof (List<OrderDTO>), 200)]
    public async Task<IActionResult> GetAsync (string query, int offset = 0, int limit = 100) {
      var email = User.Identity.Name;
      var user = await _applicationUserService.GetUserWithRolesByEmailAsync (email);

      if (user == null) {
        ModelState.AddModelError ("User", "User not found");
        return NotFound (ModelState);
      }

      var culture = _cultureService.GetCultureFromRequest (Request);

      List<Order> orders = await _orderService.GetOrdersAsync (user.Id, query, offset, limit, culture.RequestCulture.Culture.Name);

      List<OrderDTO> response = _mapper.Map<List<OrderDTO>> (orders);

      return Ok (response);
    }

    [Authorize]
    [HttpGet ("{id}", Name = "GetOrder")]
    [ProducesResponseType (typeof (OrderDTO), 200)]
    public async Task<IActionResult> GetAsync (int id) {
      var email = User.Identity.Name;
      var user = await _applicationUserService.GetUserWithRolesByEmailAsync (email);

      if (user == null) {
        ModelState.AddModelError ("User", "User not found");
        return NotFound (ModelState);
      }

      var culture = _cultureService.GetCultureFromRequest (Request);
      var order = await _orderService.GetOrderAsync (id, user.Id, culture.RequestCulture.Culture.Name);

      if (order == null) {
        return NotFound ();
      }

      OrderDTO response = _mapper.Map<OrderDTO> (order);

      return Ok (response);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType (typeof (OrderDTO), 201)]
    public async Task<IActionResult> PostAsync ([FromBody] AddOrderDTO model) {
      if (!ModelState.IsValid) {
        return BadRequest (ModelState);
      }

      var email = User.Identity.Name;
      var user = await _applicationUserService.GetUserWithRolesByEmailAsync (email);

      if (user == null) {
        ModelState.AddModelError ("User", "User not found");
        return NotFound (ModelState);
      }

      if (model.Products.Count () == 0) {
        ModelState.AddModelError ("Product", "Products is empty");
        return NotFound (ModelState);
      }

      var order = _mapper.Map<Order> (model);

      order.CustomerId = user.Id;
      order.OrderStatusId = _orderStatusService.GetOrderStatusIdByKey (OrderStatusKeys.InDevelopment);

      await _orderService.AddOrderAsync (order);

      return CreatedAtRoute (routeName: "GetOrder",
        routeValues : new { id = order.Id },
        value : _mapper.Map<OrderDTO> (order));
    }

    [Authorize]
    [HttpPut ("{id}")]
    [ProducesResponseType (typeof (OrderDTO), 201)]
    public async Task<IActionResult> PutAsync (int id, [FromBody] UpdateOrderDTO model) {
      var email = User.Identity.Name;
      var user = await _applicationUserService.GetUserWithRolesByEmailAsync (email);

      if (user == null) {
        ModelState.AddModelError ("User", "User not found");
        return NotFound (ModelState);
      }

      if (!ModelState.IsValid) {
        return BadRequest (ModelState);
      }

      Order order = await _orderService.GetOrderAsync (id, user.Id);

      if (order == null) {
        return NotFound ();
      }

      order = _mapper.Map<UpdateOrderDTO, Order> (model, order);

      await _orderService.UpdateOrderAsync (order);

      return CreatedAtRoute (routeName: "GetOrder",
        routeValues : new { id = order.Id },
        value : _mapper.Map<OrderDTO> (order));
    }

    [Authorize]
    [HttpDelete ("{id}")]
    public async Task<IActionResult> DeleteAsync (int id) {
      var email = User.Identity.Name;
      var user = await _applicationUserService.GetUserWithRolesByEmailAsync (email);

      if (user == null) {
        ModelState.AddModelError ("User", "User not found");
        return NotFound (ModelState);
      }

      var order = await _orderService.GetOrderWithProductsAsync (id, user.Id);

      if (!_orderService.OrderCanBeModified (order)) {
        ModelState.AddModelError ("Order", "Order already in progress");
        return BadRequest (ModelState);
      }

      if (order == null) {
        return NotFound ();
      }

      order.IsActive = false;

      List<Product> products = order.OrderProducts.Select (x => x.Product).ToList ();
      products.ForEach (x => x.IsActive = false);

      await _productService.UpdateMultipleProductsAsync (products);

      await _orderService.UpdateOrderAsync (order);

      return NoContent ();
    }

    [Authorize]
    [HttpGet ("around")]
    [ProducesResponseType (typeof (List<OrderDTO>), 200)]
    public async Task<IActionResult> GetOrdersAroundPositionAsync (double lat, double lon, double radius, string query, int offset = 0, int limit = 100) {

      var email = User.Identity.Name;
      var user = await _applicationUserService.GetUserWithRolesByEmailAsync (email);

      if (user == null) {
        ModelState.AddModelError ("User", "User not found");
        return NotFound (ModelState);
      }

      var culture = _cultureService.GetCultureFromRequest (Request);
      int cultureId = _cultureService.GetCultureIdByKey (culture.RequestCulture.Culture.Name);

      Point userPosition = new Point (lon, lat) { SRID = (int) LocationEnum.SRID };

      var orders = await _orderService.GetOrderAroundPositionAsync (userPosition, radius, query, offset, limit, cultureId, culture.RequestCulture.Culture.Name);

      List<OrderDTO> response = _mapper.Map<List<OrderDTO>> (orders);

      return Ok (response);

    }
  }
}