using System;
using Microsoft.AspNetCore.Mvc;
using TryBets.Users.Repository;
using TryBets.Users.Services;
using TryBets.Users.Models;
using TryBets.Users.DTO;

namespace TryBets.Users.Controllers;

[Route("[controller]")]
public class UserController : Controller
{
    private readonly IUserRepository _repository;
    public UserController(IUserRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("signup")]
    public IActionResult Post([FromBody] User user)
    {
        var newUser = _repository.Post(user);
        if (newUser == null) return BadRequest(new { message = "E-mail already used" });
        string token = new TokenManager().Generate(user);
        return Created("",new { token });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] AuthDTORequest login)
    {
        var retLogin = _repository.Login(login);
        if (retLogin == null) return BadRequest(new { message = "Authentication failed" });
        string token = new TokenManager().Generate(retLogin);
        return Ok(new { token });
    }
}