
namespace GamePlay.Boot
{
	public interface IStateNode
	{
		void OnCreate(IStateMachine machine);
		void OnEnter();
		void OnUpdate();
		void OnExit();
	}
}