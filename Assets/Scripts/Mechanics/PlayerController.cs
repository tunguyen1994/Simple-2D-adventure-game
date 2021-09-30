using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {

        public AudioClip ShootingAduio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        public static PlayerController instance;

        [SerializeField]
        GameObject[] magCount;

        [SerializeField]
        GameObject bullet;

        [SerializeField]
        GameObject[] bulletCount;

        [SerializeField]
        Transform BulletStartPos;

        public int MaxHealth;
        public int health;

        public int ammo;
        public int mag;
        public bool isReload = false;
        //public Text ammoDisplay;
        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        private bool isShoot;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        public float ShootDelay = .5f;

        bool isFacingLeft;

        public Health HealthBar;

        void Start()
        {
            mag = 3;
            for (int i = 0; i < mag; i++)
            {
                magCount[i].gameObject.SetActive(true);
            }
            ammo = 0;
            for(int i = 0; i < 5; i++)
            {
                bulletCount[i].gameObject.SetActive(false);
            }
        }

        void Awake()
        {
            MaxHealth = 3;
            health = MaxHealth;
            HealthBar.SetMaxHealth(MaxHealth);
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            if (instance == null)
            {
                instance = this;
            }

        }

        protected override void Update()
        {
            
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    isFacingLeft = true;
                }
                else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    isFacingLeft = false;
                }


                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            
            //ammoDisplay.text = ammo.ToString();
            if (Input.GetKey(KeyCode.Mouse0) && !isShoot && ammo > 0)
            {
                isShoot = true;
                //shoot bullet
                GameObject b = Instantiate(bullet);
                b.GetComponent<BulletOne>().StartShoot(isFacingLeft);
                b.transform.position = BulletStartPos.transform.position;
                Invoke("ResetShoot", ShootDelay);
                ammo -= 1;
                bulletCount[ammo].gameObject.SetActive(false);
            }
            if (ammo == 0)
            {
                isReload = true;
            }
            if (Input.GetKey(KeyCode.R))
            {
                if (isReload)
                {
                    if (mag > 0)
                    {
                        mag -= 1;
                        magCount[mag].gameObject.SetActive(false);
                        ammo = 5;
                        for (int i = 0; i < 5; i++)
                        {
                            bulletCount[i].gameObject.SetActive(true);
                        }
                        isReload = false;
                    }
                }
            }
            
            base.Update();
        }

       void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void ResetShoot()
        {
            isShoot = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                health--;
                HealthBar.SetHealth(health);
                if (health <= 0)
                {
                    Destroy(gameObject);
                }

            }

            if (collision.CompareTag("Coin"))
            {
                Destroy(collision.gameObject);
                

            }
        }


           
        
        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        public void SetMaxHealth()
        {
            MaxHealth++;
            HealthBar.SetMaxHealth(MaxHealth);
        }
        public void SetHealth()
        {
            health++;
            HealthBar.SetHealth(health);
        }

        public void SetArmor()
        {
            mag += 1;
            magCount[mag-1].gameObject.SetActive(true);
        }
    }
}