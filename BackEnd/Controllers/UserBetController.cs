using IBUAPI.Models;
using IBUAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBUAPI.Controllers;

// UserBet controller.
[ApiController]
[Route("api/[controller]")]
public class UserBetController : ControllerBase
{
    public readonly IUserBetRepository _userBets;
    private IUserRepository _users;
    private IBetRepository _bets;
    private readonly ILogger<UserBetController> _logger;

    public UserBetController(IUserBetRepository userBets,
                             IUserRepository users,
                             IBetRepository bets,
                             ILogger<UserBetController> logger)
    {
        _userBets = userBets;
        _users = users;
        _bets = bets;
        _logger = logger;
    }

    // Get all userBets.
    [HttpGet(Name = "GetAllUserBets")]
    public ActionResult<IEnumerable<UserBet>> Get()
    {
        return Ok(_userBets.GetAllUserBets());
    }

    // Get by id.
    [HttpGet("{id}", Name = "GetUserBetById")]
    public ActionResult<UserBet> Get(int id)
    {
        try
        {
            return Ok(_userBets.GetUserBetById(id));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Add UserBet by bet id and user id.
    [HttpPost("AddUserToBet/{betId}/{userId}", Name = "AddUserToBet")]
    public ActionResult<UserBet> AddUserToBet(int betId, int userId)
    {
        try
        {
            if (!_users.UserExists(userId) || !_bets.BetExists(betId))
                return BadRequest("User or bet does not exist.");
            _userBets.AddUserToBet(betId, userId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Delete UserBet by id.
    [HttpDelete("DeleteUserBet/{id}", Name = "DeleteUserBet")]
    public ActionResult<UserBet> DeleteUserBet(int id)
    {
        try
        {
            _userBets.DeleteUserBet(id);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Get all users from bet by bet id.
    [HttpGet("{betId}/participants", Name = "GetUsersFromBet")]
    public ActionResult<IEnumerable<User>> GetUsersFromBet(int betId)
    {
        try
        {
            if (!_bets.BetExists(betId))
                return BadRequest("Bet does not exist.");
            return Ok(_userBets.GetAllUsersIdsInBet(betId));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Get all bets from user by user id.
    [HttpGet("{userId}/bets", Name = "GetBetsFromUser")]
    public ActionResult<IEnumerable<Bet>> GetBetsFromUser(int userId)
    {
        try
        {
            if (!_users.UserExists(userId))
                return BadRequest("User does not exist.");
            return Ok(_userBets.GetAllBetsIdsOfUser(userId));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Confirm UserBet by user id and bet id.
    [HttpPut("{betId}/{userId}/confirm", Name = "ConfirmUserBet")]
    public ActionResult<UserBet> ConfirmUserBet(int betId, int userId)
    {
        try
        {
            if (!_users.UserExists(userId) || !_bets.BetExists(betId))
                return BadRequest("User or bet does not exist.");
            _userBets.ConfirmUserBet(userId, betId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Check if all UserBets of 1 bet are confirmed by bet id.
    [HttpGet("{betId}/confirmed", Name = "AllUserBetsConfirmed")]
    public ActionResult<bool> AllUserBetsConfirmed(int betId)
    {
        try
        {
            return Ok(_userBets.AllUserBetsConfirmed(betId));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}