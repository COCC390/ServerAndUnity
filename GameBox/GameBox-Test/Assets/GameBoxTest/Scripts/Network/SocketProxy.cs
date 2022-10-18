using com.tvd12.ezyfoxserver.client;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.factory;
using UnityEngine;

class HandshakeHandler : EzyHandshakeHandler
{
	protected override EzyRequest getLoginRequest()
	{
		return new EzyLoginRequest(
			SocketProxy.ZONE_NAME,
			SocketProxy.GetInstance().UserAuthenInfo.Username,
			SocketProxy.GetInstance().UserAuthenInfo.Password
		);
	}
}

public class SocketProxy
{
	private static readonly SocketProxy INSTANCE = new SocketProxy();

	public const string ZONE_NAME = "GameBoxServer";
	public const string PLUGIN_NAME = "GameBoxServer";

	private string host;
	private int port;
	private EzyClient client;
	private User userAuthenInfo = new User("", "");

	public string Host => host;
	public int Port => port;
	public EzyClient Client => client;
	public User UserAuthenInfo => userAuthenInfo;

	public static SocketProxy GetInstance()
	{
		return INSTANCE;
	}

	public EzyClient setup(string host, int port)
	{
		Debug.Log("Start setting up socket client...");
		this.host = host;
		this.port = port;

		var config = EzyClientConfig.builder()
			.clientName(ZONE_NAME)
			.build();
		var clients = EzyClients.getInstance();
		client = clients.newDefaultClient(config);
		var setup = client.setup();

		// Add some data handlers to setup
		setup.addDataHandler(EzyCommand.HANDSHAKE, new HandshakeHandler());

		Debug.Log("Finish setting up socket client!");
		return client;
	}
	public void login(string username, string password)
	{
		userAuthenInfo.Username = username;
		userAuthenInfo.Password = password;
		client.connect(host, port);
	}

	public void SyncPosition(Vector3 position, Quaternion rotation)
    {
		var request = EzyEntityFactory.newObject();
		request.put("angle", rotation.w);
		request.put("rotX", rotation.x);
		request.put("rotY", rotation.y);
		request.put("rotZ", rotation.z);
		request.put("posX", position.x);
		request.put("posY", position.y);
		request.put("posZ", position.z);

		var app = client.getApp();
		app.udpSend("SyncTransform", request);
    }
}
