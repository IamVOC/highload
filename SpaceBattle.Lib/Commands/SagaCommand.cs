namespace SpaceBattle.Lib;

public class SagaCommand : ICommand {
	private IList<Tuple<ICommand, ICommand>> comp;
	private ICommand pivot;
	private IList<ICommand> ret;

	public SagaCommand(IList<Tuple<ICommand, ICommand>> comp, ICommand pivot, IList<ICommand> ret) {
		this.comp = comp;
		this.pivot = pivot;
		this.ret = ret;
	}

	public void Execute() {
		int count = 0;
		try {
			for (; count < this.comp.Count; count++) {
				var (cmd, _) = this.comp[count];
				cmd.Execute();
			}
			this.pivot.Execute();
		} catch {
			count--;
			for (; count >= 0; count--) {
				var (_, compensCmd) = this.comp[count];
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
