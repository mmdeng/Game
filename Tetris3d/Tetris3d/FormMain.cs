using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using DirectInput = Microsoft.DirectX.DirectInput;

using Mmd.View;

namespace Mmd.Logic.Graphic.Mdx.Tetris3d
{
	public partial class FormMain : FormDirectx3D
	{
		protected Control _stockControl;
		protected World _world;
		protected VertexCubeMasterList _vertexBuffers;
		protected ObjectxVertex _frame;
		protected Configuration _configuration;
		protected LoopWaiter _waiterDrop;
		protected VertexLinesBottomMesh _vertexBottomMesh;
		protected BlockGeneratorList _blockGeneratorList;
		protected Block _droppingBlock;
		protected Block _targetBlock;
		protected List<int> _completeZs;
		protected LoopWaiter _waiterComplete;
		protected DirectxFont _smallFont;
		protected DirectxFont _largeFont;
		protected BitmapTexture _backgroundBitmap;
		protected Bgm _bgm;
		protected GameSystem _gameSystem;

		public FormMain()
		{
			InitializeComponent();

			this.Text = "Block";
			this.Icon = Mmd.View.ImageBuilder.GetSmallFileIcon();
			this.Size = new Size(600, 800);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.WindowState = FormWindowState.Normal;
			this.AddMonitoringKeyArrow();
			this.AddMonitoringKeyNumeric();
			this.AddMonitoringKey(Microsoft.DirectX.DirectInput.Key.Z);
			this.AddMonitoringKey(Microsoft.DirectX.DirectInput.Key.X);
			this.AddMonitoringKey(Microsoft.DirectX.DirectInput.Key.C);
			this.AddMonitoringKey(Microsoft.DirectX.DirectInput.Key.P);
			this.AddMonitoringKey(Microsoft.DirectX.DirectInput.Key.M);
			this.AddMonitoringKey(Microsoft.DirectX.DirectInput.Key.Return);
			this.AddMonitoringKey(Microsoft.DirectX.DirectInput.Key.Space);

			_stockControl = new Control();
			_stockControl.Size = new Size(120, this.Height - 50);
			_stockControl.Location = new Point(this.Width - _stockControl.Width - 20, 6);
			this.Controls.Add(_stockControl);

			Label label = new Label();
			_stockControl.Controls.Add(label);
			label.Text = "Next Block";
			label.Font = new System.Drawing.Font(FontProvider.MS_GOTHIC, 11, FontStyle.Bold);
			label.BackColor = Color.Black;
			label.ForeColor = Color.White;
			label.Top = 200;
			label.Left = 20;

		}

		private void FormMain_Load(object sender, EventArgs e)
		{
		}
		private void CreateBackground()
		{
			_backgroundBitmap = new BitmapTexture(this.ClientRectangle.Width, this.ClientRectangle.Height);
			Graphics g = Graphics.FromImage(_backgroundBitmap.Image);

			// 背景をクリア
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.Clear(Color.Transparent);

			// 背景塗りつぶし
			Color brown = Color.FromArgb(30, 0, 0);
			Rectangle rectangle = _backgroundBitmap.GetRectangle();
			Brush brush = new HatchBrush(HatchStyle.DiagonalBrick, Color.Black, brown);
			g.FillRectangle(brush, rectangle);
			brush.Dispose();

			brush = new SolidBrush(Color.Black);
			g.FillRectangle(brush, _gameSystem.RECTANGLE_GAME);
			brush.Dispose();

			Pen pen = new Pen(Color.Brown, 3);
			g.DrawRectangle(pen, _gameSystem.RECTANGLE_GAME);

			brush = new LinearGradientBrush(_gameSystem.RECTANGLE_SCORE, Color.Black, brown, 90);
			g.FillRectangle(brush, _gameSystem.RECTANGLE_SCORE);
			brush.Dispose();

			g.DrawRectangle(pen, _gameSystem.RECTANGLE_SCORE);

			brush = new LinearGradientBrush(_gameSystem.RECTANGLE_MANUAL, Color.Black, brown, 90);
			g.FillRectangle(brush, _gameSystem.RECTANGLE_MANUAL);
			brush.Dispose();

			g.DrawRectangle(pen, _gameSystem.RECTANGLE_MANUAL);

			pen.Width = 6;
			g.DrawRectangle(pen, new Rectangle(_stockControl.Location, _stockControl.Size));
			pen.Dispose();

			// 描画を完了
			g.Dispose();

			_backgroundBitmap.CreateTexture(_pallet.Device);
		}
		public override void OnInitialize()
		{
			base.OnInitialize();

			_configuration = new Configuration();

			_polarCamera = new PolarSphere(30, Math.PI / 4, -Math.PI / 8);

			// カメラの回転。Z軸方向をカメラの頭に設定
			_cameraHead = new Vector3(0, 0, -1);

			_bgm = new Bgm(this);

			_gameSystem = new GameSystem();

			SetCameraLocation();
			CreateBackground();

			// ライトの色を設定
			Light light = _pallet.MoveLight(0, new Sd(-20, -10, -40));
			light.Ambient = Color.FromArgb(40, 40, 40);
			light.Diffuse = Color.White;
			light.Update();

			light = _pallet.MoveLight(1, new Sd(20, 10, 40));
			light.Ambient = Color.FromArgb(40, 40, 40);
			light.Diffuse = Color.White;
			light.Update();

			_smallFont = new DirectxFont();
			_smallFont.Color = Color.White;
			_smallFont.Create(_pallet.Device);

			_largeFont = new DirectxFont();
			_largeFont.Color = Color.Magenta;
			_largeFont.Height = 18;
			_largeFont.Create(_pallet.Device);

			// フレーム
			_pallet.Transform.InverseZ = true;
			VertexHexahedron vertexFrame = new VertexHexahedron();
			vertexFrame.SetPositionNormal(_pallet.Device, _configuration.BlockCount * _configuration.BlockSize);
			_frame = new ObjectxVertex();
			_frame.Buffer = vertexFrame;

			// ブロック
			_blockGeneratorList = new BlockGeneratorList();

			_vertexBuffers = new VertexCubeMasterList();
			_vertexBuffers.LoadFile(_pallet.Device, @"Material\Block.bmp");
			_vertexBuffers.SetPositionNormalTextured(_pallet.Device, _configuration.BlockSize);

			_world = new World();
			_world.SetSize(_configuration.BlockCount);
			_world.Hexahedrons = _vertexBuffers;

			_vertexBottomMesh = new VertexLinesBottomMesh();
			_vertexBottomMesh.CreateVertex(_pallet.Device, _configuration);

			_waiterDrop = new LoopWaiter(1000);
			_waiterDrop.Start(_fps);

			_waiterComplete = new LoopWaiter(500, 8);
		}
		private Block CreateNextBlock()
		{
			Block block = _blockGeneratorList.Generate(_gameSystem.Level);
			block.Hexahedron = _vertexBuffers[(int)block.Type];
			return block;
		}
		public override void OnTerminate()
		{
			_smallFont.Dispose();
			_largeFont.Dispose();
			base.OnTerminate();
		}
		public override void Redraw()
		{
			// 全画面クリア
			_pallet.Clear(Color.Black);

			// スプライトの描画
			RedrawSprite();

			// 描画処理
			_pallet.Device.BeginScene();

			// スプライト以外の描画
			this.OnRedraw();

			_pallet.Device.EndScene();
			try
			{
				_pallet.Device.Present(this);
			}
			catch
			{
			}

			// Next Blocks
			_pallet.Clear(Color.Black);
			PolarSphere camera = (PolarSphere)_polarCamera.Clone();
			_polarCamera = new PolarSphere(30, Math.PI / 3.4, -Math.PI / 8);
			SetCameraLocation(Controlx.GetAspectRate(_stockControl));
			_pallet.Device.BeginScene();
			for (int i = 0; i < _blockGeneratorList.Stocks.Count; i++)
			{
				Sn position = new Sn(0, 0, -i * 3 - 5);
				Block stock = _blockGeneratorList.Stocks[i].Clone();
				stock.Hexahedron = _vertexBuffers[(int)stock.Type];
				stock.Move(position);
				stock.Draw(_pallet);
			}
			_pallet.Device.EndScene();
			try
			{
				_pallet.Device.Present(_stockControl);
			}
			catch
			{
			}
			_polarCamera = camera;
			SetCameraLocation();
		}
		public override void OnProcess()
		{
			base.OnProcess();

			if (_gameSystem.Status == GameSystem.StatusType.Idling) return;
			if (_gameSystem.Status == GameSystem.StatusType.Over) return;

			if (Listx.IsNullOrEmpty(_completeZs) == false)
			{
				if (_waiterComplete.Process() == false) return;

				_world.FlashCompletePlains(_completeZs, _waiterComplete.GetCurrentStage());

				if (_waiterComplete.IsEnd() == false) return;

				_world.RemoveCompletePlains(_completeZs);
				_completeZs = null;

				return;
			}
			if (_waiterDrop.Process() == false) return;

			bool isOk;
			if (_droppingBlock == null)
			{
				// 新しい落下ブロックを作成
				_droppingBlock = CreateNextBlock();
				isOk = _world.CheckBlock(_droppingBlock);
				if (isOk == false)
				{
					_bgm.Wmp.Stop();
					_gameSystem.SetStatus(GameSystem.StatusType.Over);
				}
			}
			if (_gameSystem.Status == GameSystem.StatusType.Pausing)
			{
				// ポーズ中
				_gameSystem.TotalScore--;
				return;
			}
			if (_waiterDrop.Span.TotalMilliseconds == 100)
			{
				// 加速中
				_gameSystem.TotalScore++;
			}
			isOk = _world.CheckMove(_droppingBlock, XYZ.Z, -1);
			if (isOk == true)
			{
				_droppingBlock.Move(XYZ.Z, -1);
				UpdateTargetBlock();
				return;
			}
			// 着地！ブロックをマージ
			_world.BindBlock(_droppingBlock);
			_droppingBlock = null;
			_targetBlock = null;

			// コンプリートチェック
			_completeZs = _world.GetCompletePlains();
			if (Listx.IsNullOrEmpty(_completeZs) == false)
			{
				_gameSystem.SetLineCount(_completeZs.Count);
				_bgm.PlayOnce(_completeZs.Count);
				_waiterComplete.Start(_fps);
			}
			else
			{
				_bgm.PlayOnce(0);
			}
		}
		public override void OnRedraw()
		{
			base.OnRedraw();

			_vertexBottomMesh.Draw(_pallet);
			_world.Draw(_pallet);

			if (_targetBlock != null)
			{
				_targetBlock.Draw(_pallet);
			}
			if (_droppingBlock != null)
			{
				_droppingBlock.Draw(_pallet);
			}
			_smallFont.Color = Color.White;

			Rectangle rectangle = _gameSystem.RECTANGLE_MANUAL;
			rectangle.X += 10;
			rectangle.Y += 10;
			_smallFont.DrawText(_gameSystem.GetStringManual(), rectangle, DrawTextFormat.Top);

			rectangle = _gameSystem.RECTANGLE_SCORE;
			rectangle.X += 10;
			rectangle.Y += 10;
			_smallFont.DrawText(_gameSystem.GetStringScore(), rectangle, DrawTextFormat.Top);

			rectangle = _gameSystem.RECTANGLE_GAME;
			rectangle.Y += 10;
			_largeFont.DrawText(_gameSystem.GetStringStatus(), rectangle, DrawTextFormat.Center);
		}
		public void OnRedrawUpper()
		{
		}
		public override void OnRedrawSprite()
		{
			_pallet.Sprite.IsBack = true;
			_pallet.Sprite.Draw(_backgroundBitmap);
			base.OnRedrawSprite();
		}
		protected override void OnKeyStateChangedNumeric(int n, bool isPressed)
		{
			_gameSystem.Level = n;
		}
		protected override void OnKeyStateChangedArrow(NewsType newsType, bool isPressed)
		{
			base.OnKeyStateChangedArrow(newsType, isPressed);
			if (isPressed == false) return;
			if (_gameSystem.Status == GameSystem.StatusType.Pausing) return;

			if (_droppingBlock == null) return;

			if (newsType == NewsType.Left)
			{
				bool isAllow = _world.CheckMove(_droppingBlock, XYZ.X, +1);
				if (isAllow == true)
				{
					_droppingBlock.Move(XYZ.X, +1);
					UpdateTargetBlock();
				}
			}
			else if (newsType == NewsType.Right)
			{
				bool isAllow = _world.CheckMove(_droppingBlock, XYZ.X, -1);
				if (isAllow == true)
				{
					_droppingBlock.Move(XYZ.X, -1);
					UpdateTargetBlock();
				}
			}
			else if (newsType == NewsType.Up)
			{
				bool isAllow = _world.CheckMove(_droppingBlock, XYZ.Y, -1);
				if (isAllow == true)
				{
					_droppingBlock.Move(XYZ.Y, -1);
					UpdateTargetBlock();
				}
			}
			else if (newsType == NewsType.Down)
			{
				bool isAllow = _world.CheckMove(_droppingBlock, XYZ.Y, +1);
				if (isAllow == true)
				{
					_droppingBlock.Move(XYZ.Y, +1);
					UpdateTargetBlock();
				}
			}
		}
		protected override void OnKeyStateChanged(DirectInput.Key key, bool isPressed)
		{
			base.OnKeyStateChanged(key, isPressed);

			// 加速
			if (key == DirectInput.Key.Space)
			{
				if (isPressed == true)
				{
					_waiterDrop = new LoopWaiter(100);
					_waiterDrop.Start(_fps);
				}
				else
				{
					_waiterDrop = new LoopWaiter(1000);
					_waiterDrop.Start(_fps);
				}
			}
			if (isPressed == false) return;

			if (key == DirectInput.Key.P)
			{
				// ポーズ、リジューム
				_gameSystem.TogglePause();
			}
			if (key == DirectInput.Key.Return)
			{
				// ゲーム開始
				if (_gameSystem.Status == GameSystem.StatusType.Pausing) return;
				if (_gameSystem.Status == GameSystem.StatusType.Playing) return;
				if (_gameSystem.Status == GameSystem.StatusType.Idling
					|| _gameSystem.Status == GameSystem.StatusType.Over)
				{
					_bgm.Wmp.PlayCurrent();
				}
				_world.SetAll(null);
				_gameSystem.SetStatus(GameSystem.StatusType.Playing);
			}
			if (key == DirectInput.Key.M)
			{
				// BGM変更
				if (_gameSystem.Status == GameSystem.StatusType.Playing)
				{
					_bgm.Wmp.PlayNext();
				}
				else
				{
					_bgm.Wmp.IncrementCurrentIndex();
				}
			}
			// 回転
			if (_droppingBlock == null) return;

			if (key == DirectInput.Key.Z)
			{
				bool isAllow = _world.CheckRotate(_droppingBlock, XYZ.Z);
				if (isAllow == true)
				{
					_droppingBlock.Rotate(XYZ.Z);
					UpdateTargetBlock();
				}
			}
			else if (key == DirectInput.Key.X)
			{
				bool isAllow = _world.CheckRotate(_droppingBlock, XYZ.X);
				if (isAllow == true)
				{
					_droppingBlock.Rotate(XYZ.X);
					UpdateTargetBlock();
				}
			}
			else if (key == DirectInput.Key.C)
			{
				bool isAllow = _world.CheckRotate(_droppingBlock, XYZ.Y);
				if (isAllow == true)
				{
					_droppingBlock.Rotate(XYZ.Y);
					UpdateTargetBlock();
				}
			}
		}
		private void UpdateTargetBlock()
		{
			_targetBlock = _droppingBlock.Clone();
			_targetBlock.Hexahedron = _vertexBuffers[(int)BlockType.Gray];
			_world.UpdateTargetBlock(_targetBlock);
		}
	}
}
