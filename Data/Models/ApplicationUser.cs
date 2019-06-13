using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Newtonsoft.Json;

namespace AllNewsServer.Data.Models {
  public class ApplicationUser : BaseEntity {
    [Key]
    [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Phone { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public int LoginAttempts { get; set; }
    public DateTime? LoginBannedUntilDateTime { get; set; }
    public string Email { get; set; }
    public byte[] Salt { get; set; }
    public string Hash { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<Order>  ReceivedOrders { get; set; }
    public virtual ICollection<Order> DeliveredOrders { get; set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<ActiveSession> ActiveSessions { get; set; }
    public bool IsBanned { get; set; }
    public string PushNotificationsToken { get; set; }
    public string Password {
      set {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create ()) {
          rng.GetBytes (salt);
        }

        // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
        string hash = Convert.ToBase64String (KeyDerivation.Pbkdf2 (
          password: value,
          salt: salt,
          prf: KeyDerivationPrf.HMACSHA1,
          iterationCount: 10000,
          numBytesRequested: 256 / 8));

        this.Salt = salt;
        this.Hash = hash;
      }
    }

    public bool isValidPassword (string password) {
      string hash = Convert.ToBase64String (KeyDerivation.Pbkdf2 (
        password: password,
        salt: this.Salt,
        prf: KeyDerivationPrf.HMACSHA1,
        iterationCount: 10000,
        numBytesRequested: 256 / 8));

      return this.Hash == hash;
    }
  }
}