﻿using HarmonyLib;
using Hazel;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.MedicMod
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    public class Protect
    {
        public static bool Prefix(KillButton __instance)
        {
            if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton) return true;
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Medic);
            if (!flag) return true;
            var role = Role.GetRole<Medic>(PlayerControl.LocalPlayer);
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            if (role.UsedAbility || role.ClosestPlayer == null) return false;
            if (role.ClosestPlayer.IsOnAlert())
            {
                Utils.RpcMurderPlayer(role.ClosestPlayer, PlayerControl.LocalPlayer);

                return false;
            }

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.Protect, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(role.ClosestPlayer.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            role.ShieldedPlayer = role.ClosestPlayer;
            role.UsedAbility = true;
            return false;
        }
    }
}
