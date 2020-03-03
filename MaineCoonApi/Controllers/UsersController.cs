using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaineCoonApi.Data;
using MaineCoonApi.Models;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using System.IO;

namespace MaineCoonApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MaineCoonApiContext _context;

        public UsersController(MaineCoonApiContext context)
        {
            _context = context;
        }
        [HttpPut("Login")]
        public async Task<IActionResult> Login() {
            using (HMACSHA256 hasher = new HMACSHA256()) {
                try {
                    string userInfoJson;
                    using (var reader = new StreamReader(Request.Body)) {
                        userInfoJson = await reader.ReadToEndAsync();
                    }
                    Console.WriteLine(userInfoJson);
                    var userInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(userInfoJson);
                    User existedUser = _context.User.Where(usr => usr.email == userInfo["userName"]).FirstOrDefault();
                    /*
                    User newuser = new User();
                    newuser.registionTime = DateTime.UtcNow;
                    newuser.sysRole = Models.User.role.Developer;
                    newuser.accountStatus = Models.User.status.Valid;
                    newuser.UserName = userInfo["userName"];
                    newuser.email = userInfo["userName"];
                    newuser.SALT = hasher.Key;
                    newuser.password = hasher.ComputeHash(Encoding.UTF8.GetBytes(userInfo["passWord"]));
                    _context.User.Add(newuser);
                    _context.SaveChanges();*/
                    if (existedUser != null) {
                        hasher.Key = existedUser.SALT;
                        byte[] password = hasher.ComputeHash(Encoding.UTF8.GetBytes(userInfo["passWord"]));
                        /*
                        existedUser.password = password;
                        _context.Entry(existedUser).State = EntityState.Modified;
                        _context.SaveChanges();
                        return Ok();
                        */
                        if (existedUser.password.SequenceEqual<byte>(password)) {
                            //password correct
                            if (existedUser.accountStatus != 0) {
                                //SUCCESS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                var claims = new List<Claim>{
                                    new Claim(ClaimTypes.Name, existedUser.UserName),
                                    new Claim(ClaimTypes.Email, existedUser.email),
                                    new Claim(ClaimTypes.Role,existedUser.sysRole.ToString()),
                                    new Claim(ClaimTypes.NameIdentifier,existedUser.Id.ToString())
                                    };
                                var claimIdentity = new ClaimsIdentity(claims: claims, authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);
                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                                userInfo.Clear();
                                userInfo["userId"] = existedUser.Id.ToString();
                                userInfo["userName"] = existedUser.UserName;
                                userInfo["sysRole"] = existedUser.sysRole.ToString();
                                return Content(JsonConvert.SerializeObject(userInfo));
                            }
                            else {
                                //account is unactivited
                                return Forbid();
                                throw new Exception("Account is banned!");
                            }
                        }
                        else {
                            return BadRequest();
                            throw new Exception("incorrect Password!");
                        }
                    }
                    else {
                        return NotFound();
                        throw new Exception("Cannot find this user!");
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }
            return BadRequest();
        }
        /*
       // GET: api/Users
       [HttpPut]
       public async Task<ActionResult<IEnumerable<User>>> GetUser()
       {
           return await _context.User.ToListAsync();
       }

       // GET: api/Users/5
       [HttpGet("{id}")]
       public async Task<ActionResult<User>> GetUser(int id)
       {
           var user = await _context.User.FindAsync(id);

           if (user == null)
           {
               return NotFound();
           }

           return user;
       }

       // PUT: api/Users/5
       // To protect from overposting attacks, please enable the specific properties you want to bind to, for
       // more details see https://aka.ms/RazorPagesCRUD.
       [HttpPut("{id}")]
       public async Task<IActionResult> PutUser(int id, User user)
       {
           if (id != user.Id)
           {
               return BadRequest();
           }

           _context.Entry(user).State = EntityState.Modified;

           try
           {
               await _context.SaveChangesAsync();
           }
           catch (DbUpdateConcurrencyException)
           {
               if (!UserExists(id))
               {
                   return NotFound();
               }
               else
               {
                   throw;
               }
           }

           return NoContent();
       }

       // POST: api/Users
       // To protect from overposting attacks, please enable the specific properties you want to bind to, for
       // more details see https://aka.ms/RazorPagesCRUD.
       [HttpPost]
       public async Task<ActionResult<User>> PostUser(User user)
       {
           _context.User.Add(user);
           await _context.SaveChangesAsync();

           return CreatedAtAction("GetUser", new { id = user.Id }, user);
       }

       // DELETE: api/Users/5
       [HttpDelete("{id}")]
       public async Task<ActionResult<User>> DeleteUser(int id)
       {
           var user = await _context.User.FindAsync(id);
           if (user == null)
           {
               return NotFound();
           }

           _context.User.Remove(user);
           await _context.SaveChangesAsync();

           return user;
       }

       private bool UserExists(int id)
       {
           return _context.User.Any(e => e.Id == id);
       }
       */
    }
}
