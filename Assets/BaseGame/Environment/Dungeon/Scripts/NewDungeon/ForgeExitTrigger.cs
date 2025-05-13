

using System.Diagnostics;

namespace LCPS.SlipForge
{
    public class ForgeExitTrigger : TriggerActionInvoker
    {
        public ExitType ExitType;

        public override void Interact()
        {
            DataTracker.Instance.LeaveDungeonEvent(false);
            
            switch (ExitType)
            {
                case ExitType.Dungeon:
                    SceneManager.ExitDungeon();
                    break;
                case ExitType.Sandbox:
                    SceneManager.ExitSandbox();
                    break;
            }

        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

    public enum ExitType
    {
        Dungeon,
        Sandbox
    }
}

