/// <summary>
/// MonoBehaviourに実装してください
/// </summary>
public interface IGameStartReciever
{
    /// <summary>
    /// 準備時間を待ってゲームが開始する際に呼ばれます。
    /// PlayerやEnemyの起動処理などを書いてください。
    /// </summary>
    public void GameStart();
}
