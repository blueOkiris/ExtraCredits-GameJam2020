using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using System.Linq;

namespace engine {
    struct GameSprite : Drawable {
        public readonly List<RectangleShape> Images;
        public IntRect CollisionMask;
        public Vector2f Position;
        public Vector2f Origin;
        public float ImageSpeed;
        public float ImageIndex;

        private RectangleShape drawShape;

        public GameSprite(GameSprite original) {
            Images = new List<RectangleShape>();

            foreach(var image in original.Images) {
                RectangleShape newImage = new RectangleShape(image.Size);
                newImage.Origin = image.Origin;
                newImage.Texture = image.Texture;
                newImage.TextureRect = image.TextureRect;

                Images.Add(newImage);
            }

            CollisionMask = new IntRect(
                original.CollisionMask.Left, original.CollisionMask.Top, 
                original.CollisionMask.Width, original.CollisionMask.Height
            );
            Position = new Vector2f(0, 0);
            ImageSpeed = original.ImageSpeed;
            ImageIndex = 0;
            drawShape = Images[0];
            Origin = drawShape.Origin;
        }

        public GameSprite(List<RectangleShape> images, Vector2f center, IntRect collisionMask, float imageSpeed) {
            Images = images;
            CollisionMask = collisionMask;
            ImageSpeed = imageSpeed;

            for(int i = 0; i < Images.Count; i++) {
                Images[i].Origin = center;
            }

            Position = new Vector2f(0, 0);
            ImageIndex = 0;
            drawShape = Images[0];
            Origin = drawShape.Origin;
        }
        
        public void Update(float deltaTime) {
            int imgIndexOld = (int) Math.Floor(ImageIndex);

            ImageIndex += ImageSpeed * deltaTime;
            if(ImageIndex >= Images.Count) {
                ImageIndex = 0;
            }

            int imgIndexNew = (int) Math.Floor(ImageIndex);
            if(imgIndexNew != imgIndexOld) {
                drawShape = Images[imgIndexNew];
            }

            drawShape.Position = Position;
            Origin = drawShape.Origin;
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Draw(drawShape);
        }

        public static List<RectangleShape> GetImagesFromTexture(Texture tex, IntRect[] cuts, Vector2f[] sizes) {
            var images = new List<RectangleShape>();

            for(int i = 0; i < cuts.Length; i++) {
                var cut = cuts[i];
                var size = sizes[i];

                var newImage = new RectangleShape(size);
                newImage.TextureRect = cut;
                newImage.Texture = tex;
                images.Add(newImage);
            }

            return images;
        }

        public static Dictionary<TileOrientation, GameSprite> CreateTileSet(
                Texture tex, Vector2i boundSize, Vector2i actualSize) {
            var tiles = new Dictionary<TileOrientation, GameSprite>();

            for(int y = 0; y < 5; y++) {
                for(int x = 0; x < 4; x++) {
                    var orientation = (TileOrientation) (y * 4 + x);

                    tiles.Add(
                        orientation,
                        new GameSprite(
                            GameSprite.GetImagesFromTexture(
                                tex,
                                new IntRect[] {
                                    new IntRect(
                                        (boundSize.X + 2) * x + 1, (boundSize.Y + 2) * y + 1, 
                                        boundSize.X, boundSize.Y)
                                },
                                new Vector2f[] {
                                    new Vector2f(actualSize.X, actualSize.Y)
                                }
                            ),
                            new Vector2f(actualSize.X / 2, actualSize.Y / 2),
                            new IntRect(0, 0, actualSize.X, actualSize.Y),
                            0
                        )
                    );
                }
            }

            return tiles;
        }
    }

    enum TileOrientation {
        Solid = 0,
        Center = 1,
        Null1 = 2,
        Null2 = 3,
        Top = 4,
        Left = 12,
        Right = 8,
        Bottom = 16,
        TopLeft = 5,
        TopRight = 6,
        BottomLeft = 9,
        BottomRight = 10,
        TopSingle = 19,
        LeftSingle = 15,
        RightSingle = 7,
        BottomSingle = 11,
        InnerBottomRight = 13,
        InnerBottomLeft = 14,
        InnerTopRight = 17,
        InnerTopLeft = 18
    }

    class Sprites {
        private static Sprites instance = null;
        
        public static Sprites getInstance() {
            if(instance == null) {
                instance = new Sprites();
            }

            return instance;
        }

        public readonly Texture Blank;
        public readonly Texture PlayerWalkTex;
        public readonly Texture GrassTileSetTex;

        public readonly GameSprite Empty;
        public readonly GameSprite PlayerWalkRight, PlayerWalkLeft;
        public readonly GameSprite PlayerStandRight, PlayerStandLeft;
        public readonly GameSprite PlayerJumpRight, PlayerJumpLeft;
        public readonly GameSprite PlayerFallRight, PlayerFallLeft;
        public readonly Dictionary<TileOrientation, GameSprite> GrassTiles;

        private Sprites() {
            Blank = new Texture("img/blank.png");
            Empty = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    Blank, 
                    new IntRect[] { new IntRect(0, 0, 64, 64) }, 
                    new Vector2f[] {
                        new Vector2f(64, 64)
                    }
                ),
                new Vector2f(0, 0),
                new IntRect(0, 0, 64, 64),
                0
            );

            PlayerWalkTex = new Texture("img/sammy-move.png");
            PlayerWalkRight = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(1, 1, 32, 32),
                        new IntRect(35, 1, 32, 32),
                        new IntRect(69, 1, 32, 32),
                        new IntRect(103, 1, 32, 32)
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                        new Vector2f(128, 128),
                        new Vector2f(128, 128),
                        new Vector2f(128, 128)
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                20
            );
            PlayerWalkLeft = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(103, 35, 32, 32),
                        new IntRect(69, 35, 32, 32),
                        new IntRect(35, 35, 32, 32),
                        new IntRect(1, 35, 32, 32)
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                        new Vector2f(128, 128),
                        new Vector2f(128, 128),
                        new Vector2f(128, 128)
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                20
            );
            PlayerStandRight = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(1, 1, 32, 32),
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                0
            );
            PlayerStandLeft = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(103, 35, 32, 32),
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                0
            );
            PlayerJumpRight = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(1, 69, 32, 32),
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                0
            );
            PlayerFallRight = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(35, 69, 32, 32),
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                0
            );
            PlayerJumpLeft = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(69, 69, 32, 32),
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                0
            );
            PlayerFallLeft = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(103, 69, 32, 32),
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                0
            );

            GrassTileSetTex = new Texture("img/simple-dirt.png");
            GrassTiles = GameSprite.CreateTileSet(GrassTileSetTex, new Vector2i(32, 32), new Vector2i(128, 128));
        }
    }
}