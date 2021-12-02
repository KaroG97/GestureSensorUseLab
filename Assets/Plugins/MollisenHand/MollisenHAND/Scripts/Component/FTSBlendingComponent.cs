using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FTS.Component
{
    public class FTSBlendingComponent : FTSGloveHand.Component
    {
        [SerializeField]
        private Animator animator;

        [SerializeField, Header("Anim Parameter Name - Thumb")]
        private string BindThumbCMC;
        [SerializeField]
        private string BindThumbMCP;
        [SerializeField]
        private string BindThumbPIP;

        [SerializeField, Header("Anim Parameter Name - Index")]
        private string BindIndexMCP;
        [SerializeField]
        private string BindIndexPIP;
        [SerializeField]
        private string BindIndexDIP;

        [SerializeField, Header("Anim Parameter Name - Middle")]
        private string BindMiddleMCP;
        [SerializeField]
        private string BindMiddlePIP;
        [SerializeField]
        private string BindMiddleDIP;

        [SerializeField, Header("Anim Parameter Name - Ring")]
        private string BindRingMCP;
        [SerializeField]
        private string BindRingPIP;
        [SerializeField]
        private string BindRingDIP;

        [SerializeField, Header("Anim Parameter Name - Pinky")]
        private string BindPinkyMCP;
        [SerializeField]
        private string BindPinkyPIP;
        [SerializeField]
        private string BindPinkyDIP;


        private FTSGloveHand glove { get; set; }
        private List<string> _bind_joint = new List<string>();

        void Awake()
        {
            _bind_joint.AddRange(new List<string> { BindThumbCMC, BindThumbMCP, BindThumbPIP,
                                                    BindIndexMCP, BindIndexPIP, BindIndexDIP,
                                                    BindMiddleMCP, BindMiddlePIP, BindMiddleDIP,
                                                    BindRingMCP, BindRingPIP, BindRingDIP,
                                                    BindPinkyMCP, BindPinkyPIP, BindPinkyDIP });
        }

        // Use this for initialization
        void Start()
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            glove = GetComponent<FTSGloveHand>();
        }

        // Update is called once per frame
        // FTSGlove 의 Joint별 값을 읽어와 Animator Setting
        public override void OnChangeState(FTSGloveHand hand, HandState state)
        {
            for (int index = 0; index < state.Length - 1; ++index) {
                if (_bind_joint[index].Length == 0)
                    continue;

                animator.SetFloat(_bind_joint[index], state[index]);
            }
        }
    }
}

