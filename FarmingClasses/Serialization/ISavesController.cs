using System.Collections.Generic;

namespace FarmingClasses.Serialization;

/// <summary>
/// Интерфейс контроллера сохранений. 
/// Позволяет при помощи сериализации/десериализации сохранять и загружать соханения игры.
/// Сохранения (класс GameSave) хранятся в словаре. Каждому имени игрока соответствует одно сохранение.
/// </summary>
public interface ISavesController : IEnumerable<KeyValuePair<string, GameSave>> {

    public int Count { get; }

    /// <summary>
    /// Получение сохранения игрока с конкретным именем.
    /// </summary>
    public GameSave this[string name] { get; }

    public bool ContainsThisPlayer(string player);

    /// <summary>
    /// Производит десериализацию данных о сохранении.
    /// В аргументах прописываются данные текущего игрока, 
    /// если требуется сохранить их в словарь после загрузки данных.
    /// </summary>
    /// <returns>Были ли успешно получены сохранения игры.</returns>
    public bool LoadSaves(string? player = null, GameSave? currentSave = null);

    /// <summary>
    /// Сохраняет текущие игры. Также можно (даже нужно) указать данные о текущей игре, 
    /// чтобы успешно сохранить данные.
    /// </summary>
    public void SaveGame(string? player = null, GameSave? save = null);

    /// <returns>Список имен всех сохраненных игроков.</returns>
    public IEnumerable<string> GetPlayers();
}
