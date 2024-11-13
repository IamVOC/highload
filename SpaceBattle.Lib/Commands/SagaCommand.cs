namespace SpaceBattle.Lib;

public class SagaCommand : ICommand {
	private IList<Tuple<ICommand, ICommand>> cmds;
	private ICommand rot;
	private IList<ICommand> ret;

	public SagaCommand(IList<Tuple<ICommand, ICommand>> cmds, ICommand rot, IList<ICommand> ret) {
		this.cmds = cmds;
		this.rot = rot;
		this.ret = ret;
	}

	public void Execute() {
		int count = 0;
		try {
			for (; count < this.cmds.Count; count++) {
				var (cmd, _) = this.cmds[count];
				cmd.Execute();
			}
			this.rot.Execute();
		} catch {
			count--;
			for (; count >= 0; count--) {
				var (_, compensCmd) = this.cmds[count];
				compensCmd.Execute();
			}
			return;
		}
		foreach(ICommand cmd in this.ret) {
			bool isCompleted = false;
			while(!isCompleted){
				try {
					cmd.Execute();
					isCompleted = true;
				} catch {}
			}
		}
	}
}
