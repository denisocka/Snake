using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace snake1
{
    class EffectManager
    {
        List<Effects> effects = new List<Effects>();


        public void AddEffects(Effects effect, Snake snake)
        {
            if (effect is ColorEffect)
            {
                var colorEffect = effects.FirstOrDefault(e => e is ColorEffect);
                if (colorEffect != null)
                {
                    colorEffect.OnRemove(snake);
                    effects.RemoveAll(e => e is ColorEffect);
                }
            }

            effect.OnApply(snake);
            effects.Add(effect);
        }

        public void RemoveEffect(Predicate<Effects> predicate,Snake snake)
        {
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                if (predicate(effects[i]))
                {
                    effects[i].OnRemove(snake);
                    effects.RemoveAt(i);
                    break;
                }
            }
        }

        public void Update(Snake snake)
        {
            for (int i =0; i<effects.Count; i++)
            {
                effects[i].OnUpdate(snake);
                if (effects[i].IsFinished)
                    effects.RemoveAt(i);
            }    
        }

    }
    public abstract class Effects
    {
        protected int Timer = 0;

        public bool IsFinished => Timer <= 0;

        protected Effects(int duration)
        {
            Timer = duration;
        }

        public virtual void OnApply(Snake snake) { }
        public virtual void OnUpdate(Snake snake)
        {
            Timer--;
            if (Timer <= 0)
                OnRemove(snake);
        }
        public virtual void OnRemove(Snake snake) { }

    }

    class ColorEffect : Effects
    {
        Color baseColor;
        Color color;
        public ColorEffect(int  duration, Color color) : base(duration)
        {
            this.color = color;
        }
        public override void OnApply(Snake snake) 
        {
            baseColor = snake.color;
            snake.color = color;
        }
        public override void OnRemove(Snake snake) 
        {
            snake.color = baseColor;
        }
    }

    class SpeedEffect : Effects
    {
        int baseSpeed;
        int speed;
        public SpeedEffect(int duration, int speed) : base(duration)
        {
            this.speed = speed;
        }
        public override void OnApply(Snake snake)
        {
            baseSpeed = snake.Speed;
            snake.Speed = speed;
        }
        public override void OnRemove(Snake snake)
        {
            snake.Speed = baseSpeed;
        }
    }

    class InvincibleEffect : Effects
    {
        int baseSpeed;
        int speed;
        public InvincibleEffect(int duration) : base(duration){}
        public override void OnApply(Snake snake)
        {
            snake.Invincible = true;
        }
        public override void OnRemove(Snake snake)
        {
            snake.Invincible = false;
        }
    }

    class DamageEffect : Effects
    {
        
        public DamageEffect(int duration) : base(duration){}

        public override void OnUpdate(Snake snake)
        {
            base.OnUpdate(snake);

            if (Timer % 20 == 0)
                snake.GetDamage(1);
        }
    }










}
