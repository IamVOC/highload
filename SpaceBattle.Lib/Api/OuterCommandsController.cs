namespace SpaceBattle.Lib;

using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class SpaceBattleController : ControllerBase
{
	private readonly ThreadManager _threadManager;

	public SpaceBattleController(ThreadManager threadManager)
	{
		_threadManager = threadManager;
	}

	[HttpPost("/push")]
	public IActionResult PushCommand([FromHeader(Name = "Game-Id")] string game)
	{
		string[] parts = game.Split('.');
		ISender sender = _threadManager.GetSender(parts[0].Trim());
		String rawJson = new StreamReader(Request.Body).ReadToEnd();
		SpaceBattle.Lib.ICommand cmd = new SendToGameCommand(rawJson, parts[1].Trim());
		sender.Send(cmd);

		return Ok();
	}
}
