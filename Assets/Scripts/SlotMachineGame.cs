using System.Collections;
using UnityEngine;

/// <summary>
/// A self-contained slot machine demo. The interface is drawn with Unity's IMGUI
/// so the project opens without requiring manually wired scene references.
/// </summary>
public class SlotMachineGame : MonoBehaviour
{
    private const int StartingCoins = 1000;
    private const int SpinCost = 10;
    private const int MatchPayout = 100;

    private readonly string[] symbolNames = { "Seven", "Cherries", "BAR", "Bell" };
    private readonly string[] textureNames = { "slot-symbol1", "slot-symbol2", "slot-symbol4", "slot-symbol3" };
    private Texture2D[] symbolTextures;
    private Texture2D backgroundTexture;
    private Texture2D cabinetTexture;
    private Texture2D panelTexture;
    private Texture2D spinTexture;
    private Texture2D leverTexture;
    private Texture2D popupTexture;

    private int[] results = { 0, 1, 2 };
    private int coins = StartingCoins;
    private bool isSpinning;
    private bool showWin;
    private string statusMessage = "Press SPIN to play!";
    private float elapsed;
    private float[] nextChange = { 0f, 0f, 0f };
    private float[] stopAt = { 1.8f, 2.35f, 2.9f };
    private GUIStyle titleStyle, labelStyle, buttonStyle, smallStyle, resultStyle;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateGame()
    {
        if (FindObjectOfType<SlotMachineGame>() != null) return;
        var gameObject = new GameObject("SlotMachineGame");
        gameObject.AddComponent<SlotMachineGame>();
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        LoadArt();
    }

    private void LoadArt()
    {
        backgroundTexture = Resources.Load<Texture2D>("SlotArt/bg_gradient");
        cabinetTexture = Resources.Load<Texture2D>("SlotArt/slot-machine3");
        panelTexture = Resources.Load<Texture2D>("SlotArt/slot_machine_Middle_box");
        spinTexture = Resources.Load<Texture2D>("SlotArt/slot_machine_buttons-03");
        leverTexture = Resources.Load<Texture2D>("SlotArt/slot-machine2");
        popupTexture = Resources.Load<Texture2D>("SlotArt/popup");
        symbolTextures = new Texture2D[textureNames.Length];
        for (int i = 0; i < textureNames.Length; i++)
            symbolTextures[i] = Resources.Load<Texture2D>("SlotArt/" + textureNames[i]);
    }

    private void Update()
    {
        if (!isSpinning) return;
        elapsed += Time.deltaTime;
        for (int i = 0; i < results.Length; i++)
        {
            if (elapsed < stopAt[i] && elapsed >= nextChange[i])
            {
                results[i] = Random.Range(0, symbolTextures.Length);
                nextChange[i] = elapsed + Random.Range(0.055f, 0.11f);
            }
        }
        if (elapsed >= stopAt[2])
        {
            isSpinning = false;
            EvaluateResult();
        }
    }

    private void EvaluateResult()
    {
        if (results[0] == results[1] && results[1] == results[2])
        {
            coins += MatchPayout;
            showWin = true;
            statusMessage = "JACKPOT!  +100 COINS";
        }
        else
        {
            showWin = false;
            statusMessage = "No match this time. Try again!";
        }
    }

    private void StartSpin()
    {
        if (isSpinning) return;
        if (coins < SpinCost)
        {
            statusMessage = "Not enough coins. Restart to play again.";
            showWin = false;
            return;
        }
        coins -= SpinCost;
        showWin = false;
        statusMessage = "Reels spinning...";
        elapsed = 0f;
        nextChange = new float[] { 0f, 0.04f, 0.08f };
        stopAt = new float[] { 1.65f, 2.25f, 2.85f };
        isSpinning = true;
    }

    private void RestartGame()
    {
        coins = StartingCoins;
        results = new int[] { 0, 1, 2 };
        statusMessage = "Fresh start! Press SPIN to play.";
        showWin = false;
        isSpinning = false;
    }

    private void SetupStyles()
    {
        titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 38, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        titleStyle.normal.textColor = new Color(1f, .79f, .28f);
        labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 23, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        labelStyle.normal.textColor = Color.white;
        buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 25, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        buttonStyle.normal.textColor = new Color(1f, .9f, .55f);
        buttonStyle.hover.textColor = Color.white;
        smallStyle = new GUIStyle(GUI.skin.label) { fontSize = 17, alignment = TextAnchor.MiddleCenter };
        smallStyle.normal.textColor = new Color(.86f, .9f, 1f);
        resultStyle = new GUIStyle(GUI.skin.label) { fontSize = 25, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        resultStyle.normal.textColor = showWin ? new Color(1f, .85f, .15f) : Color.white;
    }

    private void OnGUI()
    {
        float scale = Mathf.Min(Screen.width / 1280f, Screen.height / 720f);
        float offsetX = (Screen.width - 1280f * scale) * .5f;
        float offsetY = (Screen.height - 720f * scale) * .5f;
        Matrix4x4 oldMatrix = GUI.matrix;
        GUI.matrix = Matrix4x4.TRS(new Vector3(offsetX, offsetY, 0), Quaternion.identity, new Vector3(scale, scale, 1));
        SetupStyles();

        if (backgroundTexture != null) GUI.DrawTexture(new Rect(0, 0, 1280, 720), backgroundTexture, ScaleMode.StretchToFill);
        GUI.color = new Color(.03f, .025f, .12f, .72f);
        GUI.DrawTexture(new Rect(0, 0, 1280, 720), Texture2D.whiteTexture);
        GUI.color = Color.white;

        GUI.Label(new Rect(0, 22, 1280, 58), "ROYAL JACKPOT", titleStyle);
        GUI.Label(new Rect(0, 78, 1280, 30), "CLASSIC SLOT MACHINE  •  MATCH ALL THREE SYMBOLS", smallStyle);

        // Cabinet artwork is included as a visual frame; the reels are drawn on top.
        if (cabinetTexture != null) GUI.DrawTexture(new Rect(390, 120, 500, 382), cabinetTexture, ScaleMode.ScaleToFit, true);

        GUI.color = new Color(.06f, .04f, .20f, .95f);
        GUI.DrawTexture(new Rect(445, 245, 390, 155), Texture2D.whiteTexture);
        GUI.color = Color.white;
        for (int i = 0; i < 3; i++)
        {
            Rect reel = new Rect(458 + i * 123, 257, 112, 130);
            GUI.color = new Color(.83f, .96f, 1f);
            GUI.DrawTexture(reel, Texture2D.whiteTexture);
            GUI.color = Color.white;
            if (symbolTextures != null && symbolTextures[results[i]] != null)
                GUI.DrawTexture(new Rect(reel.x + 8, reel.y + 9, 96, 96), symbolTextures[results[i]], ScaleMode.ScaleToFit, true);
            GUI.color = new Color(.2f, .25f, .55f);
            GUI.Box(reel, GUIContent.none);
            GUI.color = Color.white;
        }

        GUI.Label(new Rect(440, 414, 400, 38), statusMessage, resultStyle);
        GUI.Label(new Rect(440, 462, 400, 32), "BALANCE:  " + coins + " COINS", labelStyle);

        GUI.enabled = !isSpinning;
        if (GUI.Button(new Rect(500, 520, 280, 76), "SPIN  •  10 COINS", buttonStyle)) StartSpin();
        GUI.enabled = true;
        if (GUI.Button(new Rect(530, 612, 220, 42), "RESTART GAME", new GUIStyle(buttonStyle) { fontSize = 18 })) RestartGame();

        GUI.Label(new Rect(80, 550, 300, 36), "3 matching symbols = +100 coins", smallStyle);
        GUI.Label(new Rect(80, 584, 300, 30), "Each spin costs 10 coins", smallStyle);
        GUI.Label(new Rect(900, 550, 300, 36), "SYMBOLS", labelStyle);
        string legend = "7     CHERRIES     BAR     BELL";
        GUI.Label(new Rect(900, 590, 300, 30), legend, smallStyle);

        if (isSpinning)
            GUI.Label(new Rect(0, 674, 1280, 28), "GOOD LUCK!", smallStyle);

        GUI.matrix = oldMatrix;
    }
}
