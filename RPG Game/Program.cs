using RPG_Game;
using RPG_Game.Models.Players;
using RPG_Game.Models.Screens;
using System.Linq.Expressions;

Player player = new Player();
Screen[] screenArr = new Screen[Enum.GetValues(typeof(Screens)).Length];

for (int i = 0; i < screenArr.Length; i++)
{
    switch (i)
    {
        case 0:
            screenArr[0] = new MainScreen();
            break;
        case 1:
            screenArr[1] = new CharacterSelect();
            break;
        case 2:
            screenArr[2] = new InGame();
            break;
        case 3:
            screenArr[3] = new Exit();
            break;
    }
}

foreach (var screen in screenArr)
{
    player =screen.OpenScreen(player);
}


  










