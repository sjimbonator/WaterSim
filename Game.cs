using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace WaterSim
{
    class Game : GameWindow
    {
        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) Exit();

            base.OnUpdateFrame(e);

        }
    }

}
