namespace Steamworks
{
	public static class GameServer
	{
		public static bool Init(uint unIP, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, string pchVersionString)
		{
			InteropHelp.TestIfPlatformSupported();
			bool flag;
			using (InteropHelp.UTF8StringHandle pchVersionString2 = new InteropHelp.UTF8StringHandle(pchVersionString))
			{
				flag = NativeMethods.SteamInternal_GameServer_Init(unIP, 0, usGamePort, usQueryPort, eServerMode, pchVersionString2);
			}
			if (flag)
			{
				flag = CSteamGameServerAPIContext.Init();
			}
			if (flag)
			{
				CallbackDispatcher.Initialize();
			}
			return flag;
		}

		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_Shutdown();
			CSteamGameServerAPIContext.Clear();
			CallbackDispatcher.Shutdown();
		}

		public static void RunCallbacks()
		{
			CallbackDispatcher.RunFrame(isGameServer: true);
		}

		public static void ReleaseCurrentThreadMemory()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_ReleaseCurrentThreadMemory();
		}

		public static bool BSecure()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamGameServer_BSecure();
		}

		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfPlatformSupported();
			return (CSteamID)NativeMethods.SteamGameServer_GetSteamID();
		}

		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamGameServer_GetHSteamPipe();
		}

		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamGameServer_GetHSteamUser();
		}
	}
}
