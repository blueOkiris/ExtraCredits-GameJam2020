using System;
using SFML.Graphics;
using SFML.System;

namespace engine {
    interface GameObject : Drawable {
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

        void Init();
        void Update(float deltaTime, KeyState keys, Room room);

        bool ShouldCull();
    }

    class MessageBox : GameObject {
        private Text message;
        private RectangleShape box;
        private bool show;

        public MessageBox(string message) {
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

            show = false;
        }

        public string GetTag() => "msg-box";

        public void Init() {}

        public void Update(float deltaTime, KeyState keys, Room room) {
            show = keys.Fire1;

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

        public bool ShouldCull() => !show;

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
    }

    class TestPlayer : GameObject {
        private Vector2f acc;
        private Vector2f vel;
        private Vector2f pos;

        private GameSprite spriteIndex;
        private float moveSpeed;
        private bool facingLeft;

        public string GetTag() => "player";

        public TestPlayer(Vector2f defaultPos) {
            spriteIndex = new GameSprite(Sprites.getInstance().PlayerStandRight);
            pos = defaultPos;

            vel = new Vector2f(0, 0);
            acc = new Vector2f(0, 0);

            moveSpeed = 512;
            facingLeft = false;
        }

        public void Init() {
            
        }

        public void Update(float deltaTime, KeyState keys, Room room) {
            spriteIndex.Position = pos;
            spriteIndex.Update(deltaTime);
            
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
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Draw(spriteIndex);
        }

        public bool ShouldCull() => false;

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
    }
}
