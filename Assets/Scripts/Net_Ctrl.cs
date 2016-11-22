using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Net_Ctrl : MonoBehaviour {
	public ArcaletGame 	ag=null;
	private const string	gguid="76be9b5c-1ca7-8d44-8a55-85103d9e268c";
	private const string	sguid="f5d0dc14-3572-4f46-b0c2-764839297a88";
	private byte[] gcert =  {0x35,0x37,0xd6,0x71,0xbc,0x52,0xd2,0x42,0x81,0x8a,0x48,0x32,0xe7,0x91,0x16,0xe,0x7d,0xb5,0x45,0xd7,0xe,0xba,0xf7,0x41,0x87,0xf7,0xe7,0x69,0xef,0x35,0x49,0x46,0x5d,0x4,0x2c,0x8c,0x9c,0xbd,0xc7,0x4a,0x82,0x4f,0x14,0x3d,0xc8,0xdd,0x8e,0x39,0x25,0x83,0xea,0x3b,0xbf,0xb0,0x76,0x48,0x82,0x1e,0x54,0x94,0x22,0x2c,0xec,0x7,0xf1,0xfa,0x30,0x46,0x8,0x14,0x15,0x42,0x98,0xf2,0x47,0xa8,0x1c,0xf7,0xbe,0x6d,0xdf,0xf8,0xc6,0xed,0xb6,0x54,0xf,0x48,0x8c,0x9a,0x44,0x86,0xca,0x3,0x5e,0xe0,0x7a,0xc2,0xe5,0xc6,0x55,0x19,0x9,0x4a,0xa4,0x6,0x53,0x9d,0x37,0xde,0x18,0x9a,0x33,0x93,0xe4,0x95,0x78,0xf,0x27,0x4c,0xaf,0xf8,0xa6,0x6,0x16,0x1f,0x98,0x58};
	private string  userid;
	private bool ArcaletGameHasLaunched=false;
	public bool isMaster=false;
	public List<Hashtable> RegPlayers=new List<Hashtable>();
	public static Net_Ctrl Instance = null;

	public GameObject remotePlayerPrefab=null;
	// Use this for initialization
	void Awake(){
		Instance = this;
		DontDestroyOnLoad(this);
		ArcaletGameHasLaunched=false;
	}
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
		if(ArcaletGameHasLaunched) ag.EventDispatcher();
	}
	public void ArcaletStartup(string userid,string passwd)
	{	

		ag=new ArcaletGame(userid,passwd,gguid,sguid,gcert);
		this.userid=userid;
		ag.onCompletion=OnArcaletLaunchCompletion;
		ag.onMessageIn=OnGameMessageIn;
		ag.onPrivateMessageIn=OnPrivateMessageIn;
		ag.onStateChanged=OnArcaletStateChanged;
		#if UNITY_WEBPLAYER
		ag.WebLaunch();
		#else
		ag.STALaunch();
		#endif
		ArcaletGameHasLaunched=true;
	}
	//不知道设置成public是否可行，要密切关注！！！！！
	//当有消息传入时
	public void OnGameMessageIn(string msg,int delay,ArcaletGame game)
	{
		//以下为自定义消息处理
		//***************************************//
		string[] s=msg.Split(':');
		if (s[0]=="new") { // 有个玩家进来了
			// 讯息格式: "new:poid/userid"
			string[] p=s[1].Split('/');
			int newPoid=int.Parse(p[0]);	    // 新玩家的私人场景
			string newUserid=p[1];			// 新玩家的userid
			if (newPoid!=ag.poid) {			// 新进的玩家不是本地玩家
				Debug.Log(p[1]+" is coming!!!");
				// 产生一个远程玩家的太空战机
				Vector3 position = new Vector3(0f,0f,0f);
				GameObject newPlayer=(GameObject)Instantiate(remotePlayerPrefab, position,Quaternion.identity);
				// 把这个玩家数据数据记起来 
				AddPlayer(newPoid,newUserid,false,newPlayer);
				// 把本地玩家的数据告诉新进玩家(用新进玩家的私人场景传讯)
				if (isMaster) {
					// 如果本地玩家是master，格式是 "player:M/poid/userid"
					ag.PrivacySend(  
						"player:M/"+ag.poid.ToString()+"/"+this.userid,newPoid);
				}
				else {
					// 如果本地玩家不master，格式是 "player:G/poid/userid"
					ag.PrivacySend(
						"player:G/"+ag.poid.ToString()+"/"+this.userid,newPoid);
				}
			}
			else {
				Debug.Log("I'm coming!");
			}
		}
		if (s[0]=="Strike") { // 进攻命令
			// 讯息格式: "Strike:poid/userid/unitid/posx/posy/posz"
			string[] p=s[1].Split('/');
			int newPoid=int.Parse(p[0]);	    // 新玩家的私人场景
			string newUserid=p[1];			// 新玩家的userid
			int unitid=int.Parse(p[2]);
			float posx = float.Parse(p[3]);
			float posy = float.Parse(p[4]);
			float posz = float.Parse(p[5]);
		}
		if (s[0]=="jump") { // 跳跃命令
			// 讯息格式: "jump:poid/unitid"
			string[] p=s[1].Split('/');
			int playerPoid=int.Parse(p[0]);	    // 玩家的私人场景
			int unitid=int.Parse(p[1]);         // 玩家的unitid
			if (playerPoid != ag.poid) {
				GameObject pgo=FindPlayer(playerPoid);
			}
		}
		if (s[0]=="atk") { // 进攻命令
			// 讯息格式: "atk:poid/unitid/atkid"
			string[] p=s[1].Split('/');
			int playerPoid=int.Parse(p[0]);	    // 新玩家的私人场景
			int unitid=int.Parse(p[1]);         // 新玩家的userid
			int atkid=int.Parse(p[2]);         // 新玩家的atkid
		}
		if (s[0]=="pos") { // 位置更新命令
			// 讯息格式: "pos:poid/unitid/posx/posy/posz/facedr"
			string[] p=s[1].Split('/');
			int playerPoid=int.Parse(p[0]);	    // 新玩家的私人场景
			int unitid=int.Parse(p[1]);         // 新玩家的userid
			float posx = float.Parse(p[2]);
			float posy = float.Parse(p[3]);
			float posz = float.Parse(p[4]);
			int facedr=int.Parse(p[5]);
			if (playerPoid != ag.poid) {
				GameObject pgo=FindPlayer(playerPoid);
				pgo.GetComponent<Player> ().resetPos (posx, posy, posz, facedr);
			}
		}
		//***************************************//
	}
	void OnArcaletStateChanged(int state,int code,ArcaletGame game)
	{
		// 开发者自定义的事件处理程序
		Debug.Log("State: " + state.ToString());
		if (state>=900){
			//Application.LoadLevel("LoginFail");
			Debug.Log("LoginFail");
			ag.Dispose();
		}

	}
	// 按下UNITY editor上的「停止」钮
	void OnApplicationQuit()
	{
		if(ArcaletGameHasLaunched) ag.Dispose();
	}
	// 将玩家数据加入RegPalyer中
	void AddPlayer(int poid,string userid,bool isMaster,GameObject gameObject)
	{	
		Hashtable np=new Hashtable();
		np.Add("userid",userid);
		np.Add("poid",poid);
		np.Add("master",isMaster);
		np.Add("gameobject",gameObject);
		lock (RegPlayers) {
			RegPlayers.Add(np);
		}
	}
	//以poid寻找玩家
	GameObject FindPlayer(int poid)
	{
		lock (RegPlayers) {
			foreach (Hashtable np in RegPlayers) {
				if ((int)np["poid"]==poid) {
					return (GameObject)np["gameobject"];	
				}
			}
		}
		return null;
	}
	// 从RegPlayers中把玩家删除
	void RemovePlayer(int poid)
	{
		lock (RegPlayers) {
			foreach (Hashtable nd in RegPlayers) {
				if ((int)nd["poid"]==poid) {
					GameObject playerGameObject=(GameObject)nd["gameobject"];
					Destroy(playerGameObject);
					RegPlayers.Remove(nd);
					return;
				}
			}
		}
	}
	// 把poid最小的玩家重新指定为游戏主控者
	void RenewMaster()
	{	
		int 		a=0;
		bool		first=true;
		Hashtable	t=null;

		// 找到poid最小的，t是该玩家的资料node
		lock (RegPlayers) {

			foreach (Hashtable nd in RegPlayers) {
				if (first) {
					a=(int)nd["poid"];
					t=nd;
				}
				else {
					if ((int)nd["poid"]<a) {
						a=(int)nd["poid"];
						t=nd;
					}
				}
				first=false;
			}
		}

		if (t != null ) {			
			// 如果找到的poid比本地玩家大,那本地玩家就是Master
			// 反之找到的poid是最小的,它就是Master

			if ((int)t["poid"]>ag.poid) {
				ag.SetPlayerStatus(0,null,null);
				ag.SendOnClose("quit:M/"+ag.poid.ToString());
				isMaster=true;
			}
			else if ((int)t["poid"]<ag.poid) {
				t["master"]=true;
			}
		}
		else {
			ag.SetPlayerStatus(0,null,null);
			ag.SendOnClose("quit:M/"+ag.poid.ToString());
			isMaster=true;
		}

	}
	// 有人传私人讯息进来了
	void OnPrivateMessageIn(string msg,int delay,ArcaletGame game)
	{
		string[] s=msg.Split(':');

		Debug.Log("[Privacy Message]"+msg);

		// 其他玩家传来他们的数据，有两种格式:
		// "player:M/poid/userid"	   	master玩家
		// "player:G/poid/userid"		一般玩家
		//		if (s[0]=="player") {
		//			string[] p=s[1].Split('/');
		//			int remotePoid=int.Parse(p[1]);
		//			string remoteUserid=p[2];
		//
		//			// 产生一个远程玩家的太空战机
		//			Vector3 position = new Vector3(0.0f,-3.219612f,0.0f);
		//			GameObject newPlayer=(GameObject)Instantiate(remotePlayerPrefab, position, Quaternion.identity);
		//
		//			// 把这个玩家数据数据记起来
		//			if (p[0]=="M") {
		//				AddPlayer(remotePoid,remoteUserid,true,newPlayer);
		//			}
		//			else if (p[0]=="G") {
		//				AddPlayer(remotePoid,remoteUserid,false,newPlayer);
		//			}
		//
		//		}
	}
	void OnArcaletLaunchCompletion(int code,ArcaletGame game) 
	{    // arcalet联机作业完成

		if (code==0) { // 联机成功
			Debug.Log("login OK2");
			// 告诉其他玩家说: 我进来了
			ag.Send("new:"+ag.poid.ToString()+"/"+userid);

			// 找看看有没有主控者
			// 只要找status为0的玩家，找得到，就是有主控者，反之，就是没有
			//ag.FindPlayersByStatus(0,FindMasterCallback,null);

		}
		else { // 联机失败
			// 进入联机失败画面
			Debug.Log("login fail2, code="+code);
			//Application.LoadLevel("LoginFail");
		}
	}
	// 找status为0的玩家
	void FindMasterCallback(int code,object s,object token)
	{
		bool Found;
		if (code==0) {
			List<Hashtable> p=(List<Hashtable>)s;
			// 传回的List是空的，表示没有找到
			if (p.Count==0) Found=false;
			else Found=true;
		}
		else {
			Found=false;
		}

		if (Found){
			// 有找到主控者(有找到status=0的玩家)
			// 预存讯息到服务器，脱机时服务器会将此讯息送至主大厅
			ag.SendOnClose("quit:G/"+ag.poid.ToString());
			Debug.Log ("not master");
		}
		else {
			// 没有找到主控者(有找到status=0的玩家)
			// 就把本地的玩家当成主控者(也就是把status设为0)
			// 这样子其他的玩家就可用FindPlayersByStatus来寻找主控者
			ag.SetPlayerStatus(0,null,null);
			Debug.Log ("is master");
			// 这个变量要让其它对象参考
			isMaster=true;

			// 预存讯息到服务器，脱机时服务器会将此讯息送至主大厅
			ag.SendOnClose("quit:M/"+ag.poid.ToString());
		}

		// 已经确认谁主控者了
		// 接着进入游戏的level 1
		//Application.LoadLevel("Level1");
	}
}
