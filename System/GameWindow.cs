using System.Numerics;

public class GameWindow : Window
{
    private Camera3D _camera;
    private List<Mesh> _meshes = [];

    private readonly Vertex[] _vertices1 =
    [
        new(-1, -1, -1, '*', ConsoleColor.Red),   // 0
        new( 1, -1, -1, '*', ConsoleColor.Red),   // 1
        new( 1,  1, -1, '*', ConsoleColor.Red),   // 2
        new(-1,  1, -1, '*', ConsoleColor.Red),   // 3
        new(-1, -1,  1, '*', ConsoleColor.Red), // 4
        new( 1, -1,  1, '*', ConsoleColor.Red), // 5
        new( 1,  1,  1, '*', ConsoleColor.Red), // 6
        new(-1,  1,  1, '*', ConsoleColor.Red), // 7
    ];
    
    private readonly Vertex[] _vertices2 =
    [
        new(-1, -1, -1, '*', ConsoleColor.Yellow),   // 0
        new( 1, -1, -1, '*', ConsoleColor.Yellow),   // 1
        new( 1,  1, -1, '*', ConsoleColor.Yellow),   // 2
        new(-1,  1, -1, '*', ConsoleColor.Yellow),   // 3
        new(-1, -1,  1, '*', ConsoleColor.Yellow), // 4
        new( 1, -1,  1, '*', ConsoleColor.Yellow), // 5
        new( 1,  1,  1, '*', ConsoleColor.Yellow), // 6
        new(-1,  1,  1, '*', ConsoleColor.Yellow), // 7
    ];
    
    private readonly Vertex[] _vertices3 =
    [
        new(-1, -1, -1, '*', ConsoleColor.Magenta),   // 0
        new( 1, -1, -1, '*', ConsoleColor.Magenta),   // 1
        new( 1,  1, -1, '*', ConsoleColor.Magenta),   // 2
        new(-1,  1, -1, '*', ConsoleColor.Magenta),   // 3
        new(-1, -1,  1, '*', ConsoleColor.Magenta), // 4
        new( 1, -1,  1, '*', ConsoleColor.Magenta), // 5
        new( 1,  1,  1, '*', ConsoleColor.Magenta), // 6
        new(-1,  1,  1, '*', ConsoleColor.Magenta), // 7
    ];

    private readonly int[] _indices =
    [
        // Задняя грань
        0, 1, 2,
        0, 2, 3,

        // Передняя грань
        4, 6, 5,
        4, 7, 6,

        // Нижняя грань
        0, 5, 1,
        0, 4, 5,

        // Верхняя грань
        3, 2, 6,
        3, 6, 7,

        // Левая грань
        0, 3, 7,
        0, 7, 4,

        // Правая грань
        1, 5, 6,
        1, 6, 2
    ];
    
    public GameWindow(int realWidth, int realHeight) : base(realWidth, realHeight)
    {
        
    }

    protected override void OnLoad()
    {
        Console.Title = "OpenTK-style Console";
        Console.CursorVisible = false;

        CursorUtils.Hide();

        Input.InitializeMouse(ConsoleWidth * 8, ConsoleHeight * 16);

        _camera = new Camera3D
        {
            Position = new Vector3(5, 3, -20)
        };

        var mesh1 = new Mesh(_vertices1, _indices)
        {
            Transform =
            {
                Position = new Vector3(0, 0, 0),
                Scale = new Vector3(1, 1, 1) * 4f
            }
        };
        
        var mesh2 = new Mesh(_vertices2, _indices)
        {
            Transform =
            {
                Position = new Vector3(0, 0, 30),
                Scale = new Vector3(1, 1, 1) * 4f
            }
        };
        
        
        var mesh3 = new Mesh(_vertices3, _indices)
        {
            Transform =
            {
                Position = new Vector3(30, 0, 30),
                Scale = new Vector3(1, 1, 1) * 4f
            }
        };

        _meshes.Add(mesh1);
        _meshes.Add(mesh2);
        _meshes.Add(mesh3);
    }

    protected override void OnUpdateFrame()
    {
        Clear();

        var rot = Matrix4x4.CreateFromYawPitchRoll(_camera.Rotation.Y, _camera.Rotation.X, _camera.Rotation.Z);

        var forward = System.Numerics.Vector3.Transform(-System.Numerics.Vector3.UnitZ, rot).ToCustom();
        var right = System.Numerics.Vector3.Transform(System.Numerics.Vector3.UnitX, rot).ToCustom();
        var up = System.Numerics.Vector3.Transform(System.Numerics.Vector3.UnitY, rot).ToCustom();

        float speed = 10f * DeltaTime;

        if (RawInput.IsKeyDown(ConsoleKey.W)) _camera.Position += forward * speed;
        if (RawInput.IsKeyDown(ConsoleKey.S)) _camera.Position -= forward * speed;
        if (RawInput.IsKeyDown(ConsoleKey.A)) _camera.Position -= right * speed;
        if (RawInput.IsKeyDown(ConsoleKey.D)) _camera.Position += right * speed;
        if (RawInput.IsKeyDown(ConsoleKey.Spacebar)) _camera.Position += up * speed;
        if (RawInput.IsKeyDown(ConsoleKey.Z)) _camera.Position -= up * speed;

        if (RawInput.IsKeyDown(ConsoleKey.UpArrow)) _camera.Rotation.X += 2f * DeltaTime;
        if (RawInput.IsKeyDown(ConsoleKey.DownArrow)) _camera.Rotation.X -= 2f * DeltaTime;
        
        if (RawInput.IsKeyDown(ConsoleKey.E)) _camera.Rotation.Y -= 3f * DeltaTime;
        if (RawInput.IsKeyDown(ConsoleKey.Q)) _camera.Rotation.Y += 3f * DeltaTime;

        float mouseSensitivity = 0.002f;
        _camera.Rotation.Y -= Input.MouseDeltaX * mouseSensitivity;
        _camera.Rotation.X -= Input.MouseDeltaY * mouseSensitivity;
        
        foreach (var mesh in _meshes)
        {
            mesh.Draw(this, _camera);
        }
        
        _meshes[2].Transform.Rotation += new Vector3(0f, 1f, 0f) * 0.01f;
 
        //Console.WriteLine($"C : {_camera.Position} {_camera.Rotation} | O : {_meshes[0].Transform.Position}");
    }
}

