namespace SpaceBattle.Lib;

public class SagaCommand : ICommand {
	private IList<Tuple<ICommand, ICommand>> cmds;

	public SagaCommand(IList<Tuple<ICommand, ICommand>> cmds) {
		this.cmds = cmds;
	}

	public void Execute() {
		int count = 0;
		try {
			for (; count < this.cmds.Count; count++) {
				var (cmd, _) = this.cmds[count];
				cmd.Execute();
			}
		} catch {
			count--;
			for (; count >= this.cmds.Count; count++) {
				var (_, compensCmd) = this.cmds[count];
				compensCmd.Execute();
			}
		}
	}
}
