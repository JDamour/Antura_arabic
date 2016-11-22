using UnityEngine;

namespace EA4S.Assessment
{
    public class AnturaFactory : MonoBehaviour
    {
        [Header("Prefabs")]
        public AnturaAnimationController anturaPrefab;
        public ParticleSystem sleepingParticles;

        [Header("Positions")]
        public Transform anturaGoAway;
        public Transform sleepingStart;

        #region Instance
        /////////////////
        // Singleton Pattern: Ok as long as factory has not other singletons as dependencies
        static AnturaFactory instance;
        public static AnturaFactory Instance
        {
            get
            {
                return instance;
            }
        }
        

        void Awake()
        {
            instance = this;
        }
        /////////////////
        #endregion

        public AssessmentAnturaController SleepingAntura()
        {
            var antura = 
                Instantiate(    anturaPrefab, sleepingStart.position, 
                                sleepingStart.rotation) as AnturaAnimationController;

            var go = antura.gameObject;
            var box = go.AddComponent< BoxCollider>();
            go.SetLayerRecursive( AnturaLayers.ModelsOverUI);


            box.center = new Vector3( 0, 2.9f, -1.3f);
            box.size = new Vector3( 5.38f, 5.61f, 9.57f);

            var controller = go.AddComponent< AssessmentAnturaController>();
            controller.sleepingParticles = sleepingParticles;
            controller.anturaDestination = anturaGoAway;
            controller.antura = antura;

            return controller;
        }
    }
}