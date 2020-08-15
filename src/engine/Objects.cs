using System;
using SFML.Graphics;
using SFML.System;

namespace engine {
    interface GameObject : Drawable, IComparable {
        Vector2f GetAcceleration();
        Vector2f GetVelocity();
        Vector2f GetPosition();
        void SetAcceleration(Vector2f acc);
        void SetVelocity(Vector2f vel);
        void SetPosition(Vector2f pos);
        
        GameSprite GetSpriteIndex();
        IntRect GetMask();
        void SetSpriteIndex(GameSprite spr);
        void SetMask(IntRect mask);

        string GetTag();
        int GetDepth();

        void Init();
        void Update(float deltaTime, KeyState keys, Room room);

        bool ShouldCull(Room room);
    }

    class SimpleBlock : GameObject {
        private Vector2f pos;
        private GameSprite box;

        public SimpleBlock(Vector2f position) {
            pos = position;
            box = new GameSprite(Sprites.getInstance().SimpleGrassBlock);
            box.Position = position;
            box.Update(0);
        }

        public string GetTag() => "block";
        public int GetDepth() => int.MinValue;

        public void Init() {}
        public void Update(float deltaTime, KeyState keys, Room room) {}

        public void Draw(RenderTarget target, RenderStates states) {
            target.Draw(box);
        }

        public bool ShouldCull(Room room) {
            var view = room.GetViewPosition();

            if((pos.X < (int) view.X - 64) || (pos.X > (int) view.X + Settings.ScreenSize.X + 64) ||
                    (pos.Y < (int) view.Y - 64) || (pos.Y > (int) view.Y + Settings.ScreenSize.Y + 64)) {
                return true;
            }

            return false;
        }

        public Vector2f GetPosition() => pos;
        public IntRect GetMask() => box.CollisionMask;
        public GameSprite GetSpriteIndex() => box;

        public Vector2f GetVelocity() => new Vector2f(0, 0);
        public Vector2f GetAcceleration() => new Vector2f(0, 0);
        public void SetPosition(Vector2f pos) {}
        public void SetVelocity(Vector2f vel) {}
        public void SetAcceleration(Vector2f acc) {}
        public void SetSpriteIndex(GameSprite spr) {}
        public void SetMask(IntRect mask) {}

        public int CompareTo(object obj) {
            return GetDepth().CompareTo((obj as GameObject).GetDepth());
        }
    }

    class MessageBox : GameObject {
        public bool Show;

        private Text message;
        private RectangleShape box;
        private string id;

        public MessageBox(string message, string id) {
            this.id = id;

            this.message = new Text(message, Settings.DefaultFont, 30);
            this.message.Origin = new Vector2f(
                this.message.GetLocalBounds().Width / 2,
                this.message.GetLocalBounds().Height / 2
            );

            box = new RectangleShape(
                new Vector2f(
                    Settings.ScreenSize.X * 0.75f,
                    Settings.ScreenSize.Y * 0.75f
                )
            );
            box.Origin = new Vector2f(
                box.GetLocalBounds().Width / 2,
                box.GetLocalBounds().Height / 2
            );
            box.FillColor = Settings.DefaultFillColor;
            box.OutlineColor = Settings.DefaultOutlineColor;
            box.OutlineThickness = 30;

            Show = false;
        }

        public string GetTag() => "msg-box-" + id;
        public int GetDepth() => int.MaxValue;

        public void Init() {}

        public void Update(float deltaTime, KeyState keys, Room room) {
            box.Position = new Vector2f(
                room.GetViewPosition().X + Settings.ScreenSize.X / 2,
                room.GetViewPosition().Y + Settings.ScreenSize.Y / 2
            );
            message.Position = box.Position;
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Draw(box);
            target.Draw(message);
        }

        public bool ShouldCull(Room room) => !Show;

        public Vector2f GetPosition() => new Vector2f(0, 0);
        public Vector2f GetVelocity() => new Vector2f(0, 0);
        public Vector2f GetAcceleration() => new Vector2f(0, 0);
        public IntRect GetMask() => new IntRect(0, 0, 0, 0);
        public GameSprite GetSpriteIndex() => Sprites.getInstance().Empty;
        public void SetPosition(Vector2f pos) {}
        public void SetVelocity(Vector2f vel) {}
        public void SetAcceleration(Vector2f acc) {}
        public void SetSpriteIndex(GameSprite spr) {}
        public void SetMask(IntRect mask) {}

        public int CompareTo(object obj) {
            return GetDepth().CompareTo((obj as GameObject).GetDepth());
        }
    }

    class TestPlayer : GameObject {
        private Vector2f acc;
        private Vector2f vel;
        private Vector2f pos;

        private GameSprite spriteIndex;
        private float moveSpeed;
        private bool facingLeft;

        public string GetTag() => "player";
        public int GetDepth() => 1;

        public TestPlayer(Vector2f defaultPos) {
            spriteIndex = new GameSprite(Sprites.getInstance().PlayerStandRight);
            pos = defaultPos;
            spriteIndex.Position = pos;
            spriteIndex.Update(0);

            vel = new Vector2f(0, 0);
            acc = new Vector2f(0, 0);

            moveSpeed = 256;
            facingLeft = false;
        }

        public void Init() {
            
        }

        public void Update(float deltaTime, KeyState keys, Room room) {
            if(keys.Up && pos.Y > 0) {
                if(vel.Y != -moveSpeed) {
                    vel.Y = -moveSpeed;

                    if(facingLeft) {
                        spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkLeft);
                    } else {
                        spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkRight);
                    }
                }
            } else if(keys.Down && pos.Y < room.GetSize().Y) {
                if(vel.Y != moveSpeed) {
                    vel.Y = moveSpeed;

                    if(facingLeft) {
                        spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkLeft);
                    } else {
                        spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkRight);
                    }
                }
            } else {
                vel.Y = 0;
            }

            if(keys.Left && pos.X > 0) {
                if(vel.X != -moveSpeed) {
                    vel.X = -moveSpeed;
                    spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkLeft);
                }

                facingLeft = true;
            } else if(keys.Right && pos.X < room.GetSize().X) {
                if(vel.X != moveSpeed) {
                    vel.X = moveSpeed;
                    spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkRight);
                }
                
                facingLeft = false;
            } else {
                vel.X = 0;
            }

            if(vel.X == 0 && vel.Y == 0) {
                if(facingLeft) {
                    spriteIndex = new GameSprite(Sprites.getInstance().PlayerStandLeft);
                } else {
                    spriteIndex = new GameSprite(Sprites.getInstance().PlayerStandRight);
                }
            }

            var blocks = Engine.FindGameObjectsByTag("block", room);
            if(Engine.IsPlaceMeeting(new Vector2f(pos.X + vel.X * 2 * deltaTime, pos.Y - 1), this, blocks)
                    && Engine.IsPlaceMeeting(new Vector2f(pos.X + vel.X * 2 * deltaTime, pos.Y + 1), this, blocks)) {
                vel.X = 0;
            }
            if(Engine.IsPlaceMeeting(new Vector2f(pos.X - 1, pos.Y + vel.Y * 2 * deltaTime), this, blocks)
                    && Engine.IsPlaceMeeting(new Vector2f(pos.X + 1, pos.Y + vel.Y * 2 * deltaTime), this, blocks)) {
                vel.Y = 0;
            }

            spriteIndex.Position = pos;
            spriteIndex.Update(deltaTime);
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Draw(spriteIndex);
        }

        public bool ShouldCull(Room room) => false;

        public Vector2f GetAcceleration() => acc;
        public Vector2f GetVelocity() => vel;
        public Vector2f GetPosition() => pos;
        public void SetAcceleration(Vector2f acc) => this.acc = acc;
        public void SetVelocity(Vector2f vel) => this.vel = vel;
        public void SetPosition(Vector2f pos) => this.pos = pos;

        public GameSprite GetSpriteIndex() => spriteIndex;
        public IntRect GetMask() => spriteIndex.CollisionMask;
        public void SetSpriteIndex(GameSprite spr) => this.spriteIndex = spr;
        public void SetMask(IntRect mask) {}

        public int CompareTo(object obj) {
            return GetDepth().CompareTo((obj as GameObject).GetDepth());
        }
    }
}
