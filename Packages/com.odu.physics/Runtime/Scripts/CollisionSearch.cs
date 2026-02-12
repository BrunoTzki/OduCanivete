using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//Coloquei OduPhysics pq tava conflitando com o Physics da Unity, depois temos que ver isso aí
namespace Odu.Utilities.OduPhysics
{
    public class CollisionSearch
    {
        /// <summary>
        /// Itera sobre uma lista genérica de T, onde T é Component, e calcula a distância da posição origem para determinar qual elemento é o mais próximo.
        /// </summary>
        /// <typeparam name="T">Component</typeparam>
        /// <param name="Position">Origem da verificação de distância.</param>
        /// <param name="Entities">Array com todas as entidades a serem verificadas e comparadas.</param>
        /// <returns></returns>
        public static T FindClosestEntity<T>(Vector3 Position, T[] Entities) where T : Component
        {
            if (Entities.Length == 0)
            {
                return null;
            }
            T closestEntity = Entities[0];

            float distanceComparator = Mathf.Infinity;
            foreach (T entity in Entities)
            {
                float distance = Vector3.Distance(entity.transform.position, Position);
                if (distance < distanceComparator)
                {
                    closestEntity = entity;
                    distanceComparator = distance;
                }
            }

            return closestEntity;
        }

        /// <summary>
        /// Retorna uma lista de objetos encontrados dentro de uma secção de uma esfera recebendo uma direção como parâmetro.
        /// </summary>
        /// <param name="SphereRadius">Raio da esfera a ser verificada.</param>
        /// <param name="Angle">Ângulo da secção de esfera. Caso seja 360, a esfera será verificada por completo</param>
        /// <param name="Position">Posição central onde a esfera será posicionada.</param>
        /// <param name="Layer">Máscara de layers a serem consideradas na busca.</param>
        /// <param name="Direction">Direção para qual as colisões serão checadas, se não definida, o padrão é Vector3.forward</param>
        /// <returns></returns>
        public static List<Collider> FindEntitiesWithinSphereSection(float SphereRadius, float Angle, Vector3 Position, LayerMask Layer,[Optional] Vector3 Direction)
        {
            //Define "para frente" como direção padrão
            if(Direction == null)
            {
                Direction = Vector3.forward;
            }
            //Variaveis para achar os alvos
            Collider[] overlapHitBoxes = new Collider[10];
            List<Collider> validTargets = new List<Collider>();
            int entitiesFound = Physics.OverlapSphereNonAlloc(Position, SphereRadius, overlapHitBoxes, Layer);
            for (int i = 0; i < entitiesFound; i++)
            {
                Vector3 targetDir = overlapHitBoxes[i].transform.position - Position;
                float angle = Vector3.Angle(targetDir, Direction);
                if (angle <= Angle / 2)
                {
                    validTargets.Add(overlapHitBoxes[i]);
                }
            }
            return validTargets;
        }

        /// <summary>
        /// Busca pela entidade mais próxima dentro de uma secção de esfera utilizando um vetor de direção para determinar o sentido da busca.
        /// </summary>
        /// <param name="SphereRadius">Raio da esfera a ser verificada.</param>
        /// <param name="Angle">Ângulo da secção de esfera. Caso seja 360, a esfera será verificada por completo</param>
        /// <param name="Position">Posição central onde a esfera será posicionada.</param>
        /// <param name="Layer">Máscara de layers a serem consideradas na busca.</param>
        /// <param name="Direction">Direção para qual as colisões serão checadas, se não definida, o padrão é Vector3.forward</param>
        /// <returns></returns>
        public static Collider FindClosestEntityWithinSphereSection(float SphereRadius, float Angle, Vector3 Position, LayerMask Layer,[Optional] Vector3 Direction)
        {
            Collider[] entities = FindEntitiesWithinSphereSection(SphereRadius, Angle, Position, Layer, Direction).ToArray();
            return FindClosestEntity<Collider>(Position, entities);
        }

        /// <summary>
        /// Retorna uma lista de objetos encontrados dentro de uma região de cubo.
        /// </summary>
        /// <param name="BoxSize">Tamanho do cubo, determinado pelas 3 dimensões.</param>
        /// <param name="Angle">Ângulo da secção do cubo. Caso seja 360, o cubo será verificado por completo</param>
        /// <param name="Center">Posição central onde o cubo será posicionado.</param>
        /// <param name="Rotation">Rotação do cubo.</param>
        /// <param name="Layer">Máscara de layers a serem consideradas na busca.</param>
        /// <returns></returns>
        public static List<Collider> FindEntitiesWithinBoxSection(Vector3 BoxSize, float Angle, Transform Center, Quaternion Rotation, LayerMask Layer)
        {
            //Variaveis para achar os alvos
            Collider[] overlapHitBoxes = new Collider[10];
            List<Collider> validTargets = new List<Collider>();
            int entitiesFound = Physics.OverlapBoxNonAlloc(Center.position, BoxSize, overlapHitBoxes, Rotation, Layer);
            for (int i = 0; i < entitiesFound; i++)
            {
                Vector3 targetDir = overlapHitBoxes[i].transform.position - Center.position;
                float angle = Vector3.Angle(targetDir, Center.forward);
                if (angle <= Angle / 2)
                {
                    validTargets.Add(overlapHitBoxes[i]);
                }
            }
            return validTargets;
        }

        /// <summary>
        /// Lança mútiplos raycasts em uma direção dentro de um ângulo determinado. 
        /// </summary>
        /// <param name="CastDistance">Alcance dos raycasts. Distância máxima de busca.</param>
        /// <param name="Angle">Ângulo de abertura dos raycasts.</param>
        /// <param name="Center">Posição central de origem dos raycasts</param>
        /// <param name="Layer">Máscara de layers a serem consideradas na busca.</param>
        /// <param name="RaycastCount">Definição da busca. Determina quantos raycasts serão lançados dentro do raio de busca.</param>
        /// <returns></returns>
        public static List<Collider> FindEntitiesWithinAngleWithRaycast(float CastDistance, float Angle, Transform Center, LayerMask Layer, int RaycastCount, [Optional] bool DebugView)
        {
            //Variaveis para definir o ponto inicial
            Quaternion InicialRotaion = Quaternion.AngleAxis(-Angle / 2, Center.up);
            Vector3 StartVector = InicialRotaion * Center.forward;
            //Vari?vel para o retorno
            List<Collider> validTargets = new List<Collider>();
            //Vari?vel para o for
            Quaternion RaycastRotation = Quaternion.AngleAxis(Angle / (RaycastCount - 1), Center.up);
            Vector3 RaycastDirection = StartVector;
            RaycastHit hit;
            for (int i = 0; i < RaycastCount; i++)
            {

                if (Physics.Raycast(Center.position, RaycastDirection, out hit, CastDistance, Layer))
                {
                    if (DebugView)
                    {
                        Debug.DrawRay(Center.position, RaycastDirection * hit.distance, Color.red, 1f);
                    }
                    validTargets.Add(hit.collider);
                }
                else if (DebugView)
                {
                    Debug.DrawRay(Center.position, RaycastDirection * CastDistance, Color.green, 1f);
                }
                RaycastDirection = RaycastRotation * RaycastDirection;

            }
            return validTargets;
        }
    }
}
