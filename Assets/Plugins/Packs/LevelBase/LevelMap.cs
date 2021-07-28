namespace Lance.Pattern.LevelBase
{
    public class LevelMap : Unit
    {
        public LevelRoot Root { get; set; }

        public override void DarknessRise()
        {
            var units = GetComponentsInChildren<Unit>();
            foreach (var unit in units)
            {
                if (unit != this) unit.DarknessRise();
            }
        }

        public override void LightReturn()
        {
            // todo
            var units = GetComponentsInChildren<Unit>();
            foreach (var unit in units)
            {
                if (unit != this) unit.LightReturn();
            }
        }
    }
}