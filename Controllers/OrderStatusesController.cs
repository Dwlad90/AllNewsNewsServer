using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tenli.Server.Data;
using Tenli.Server.Data.Constants;
using Tenli.Server.Data.DTOs.OrderStatus;
using Tenli.Server.Data.Models;
using Tenli.Server.Services;

namespace Tenli.Server.Controllers {
  [Route ("api/v1/[controller]")]
  public class OrderStatusesController : Controller {
    private readonly IMapper _mapper;
    private readonly OrderStatusService _orderStatusService;
    private readonly CultureService _cultureService;

    public OrderStatusesController (IMapper mapper, OrderStatusService orderStatusService, CultureService cultureService) {
      _mapper = mapper;
      _orderStatusService = orderStatusService;
      _cultureService = cultureService;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType (typeof (List<OrderStatusDTO>), 200)]
    public async Task<IActionResult> GetAsync () {
      var culture = _cultureService.GetCultureFromRequest (Request);
      var assignments = await _orderStatusService.GetOrderStatusesAsync (culture.RequestCulture.Culture.Name);

      var response = _mapper.Map<List<OrderStatus>, List<OrderStatusDTO>> (assignments);

      return Ok (response);
    }
  }
}