namespace TownOfUs.Roles
{
    public class Janitor : Role
    {
        public KillButton _cleanButton;

        public Janitor(PlayerControl player) : base(player)
        {
            Name = "Janitor";
            ImpostorText = () => "Clean up bodies";
            TaskText = () => "Clean bodies to prevent Crewmates from discovering them.";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Janitor;
            Faction = Faction.Impostors;
        }

        public DeadBody CurrentTarget { get; set; }

        public KillButton CleanButton
        {
            get => _cleanButton;
            set
            {
                _cleanButton = value;
                ExtraButtons.Clear();
                ExtraButtons.Add(value);
            }
        }
    }
}