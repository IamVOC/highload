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

	[HttpPost("/game/transit")]
	public IActionResult TransitGame([FromHeader(Name = "Game-Id")] string game,
			[FromBody] TargetServerSchema serverSchema)
	{
		string[] parts = game.Split('.');
		ISender sender = _threadManager.GetSender(parts[0].Trim());
		SpaceBattle.Lib.ICommand cmd = new TransitGameCommand(serverSchema, parts[1].Trim());
		sender.Send(cmd);

		return Ok();
	}
	[HttpPost("/game/catch")]
	public IActionResult CatchGame([FromHeader(Name = "Thread-Id")] string thread, 
			[FromBody] SerializedGameSchema gameSchema)
	{
		ISender sender = _threadManager.GetSender(thread);
		SpaceBattle.Lib.ICommand cmd = new CatchGameCommand(gameSchema, thread);
		sender.Send(cmd);

		return Ok();
	}
}
