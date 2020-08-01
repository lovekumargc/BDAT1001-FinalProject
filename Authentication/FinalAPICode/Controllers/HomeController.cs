using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FinalAPICode.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        List<Item> items = new List<Item>();

        public HomeController()
        {
            items.Add(new Item
            {
                Id = 1,
                ItemTitle = "Item 1",
                ItemSKU = "SKU 001",
                ItemDetail = "This is a testing detail for item 1",
                ItemPrice = 210
            });

            items.Add(new Item
            {
                Id = 2,
                ItemTitle = "Item 2",
                ItemSKU = "SKU 002",
                ItemDetail = "This is a testing detail for item 2",
                ItemPrice = 150
            });

            items.Add(new Item
            {
                Id = 3,
                ItemTitle = "Item 3",
                ItemSKU = "SKU 003",
                ItemDetail = "This is a testing detail for item 3",
                ItemPrice = 200
            });

            items.Add(new Item
            {
                Id = 4,
                ItemTitle = "Item 4",
                ItemSKU = "SKU 004",
                ItemDetail = "This is a testing detail for item 4",
                ItemPrice = 145
            });

            items.Add(new Item
            {
                Id = 5,
                ItemTitle = "Item 5",
                ItemSKU = "SKU 005",
                ItemDetail = "This is a testing detail for item 5",
                ItemPrice = 124
            });

        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Authenticate")]
        public IActionResult Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("granny", "cookie")
            };

            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audiance,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { access_token = tokenJson });
        }
        [Authorize]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(new { data = items });
        }
        [Authorize]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(new { data = items.Where(c => c.Id == id).FirstOrDefault() });
        }
        [Authorize]
        [Route("Secret")]
        public IActionResult Secret()
        {
            return View();
        }
    }
}
