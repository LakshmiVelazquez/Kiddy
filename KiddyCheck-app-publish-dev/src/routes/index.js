import { createRouter, createWebHistory } from 'vue-router'
//import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'Login',
      component: () => import('../views/Login/Login.vue')
    },
    {
      path: '/Navegacion',
      name: 'Navegacion',
      component: () => import('../views/Navegacion/Navegacion.vue'),
      children: [
        {
          path: '/home',
          name: 'home',
          
          component: () => import('../views/Home.vue')
        },
        {
          path: '/usuarios',
          name: 'usuarios',
          component: () => import('../views/Usuarios.vue')
        },
        {
          path: '/Grado',
          name: 'Grados',
          component: () => import('../views/Grado/Grados.vue')
        },
        {
          path: '/Grupo',
          name: 'Grupos',
          component: () => import('../views/Grupo/Grupos.vue')
        },
        {
          path: '/alumnos',
          name: 'alumnos',
          component: () => import('../views/Alumnos/index.vue')
        },
        {
          path: '/Maestros',
          name: 'Maestros',
          component: () => import('../views/Maestro/Maestros.vue')
        },
        {
          path: '/AltasMaterias',
          name: 'Materias',
          component: () => import('../views/AltasMaterias/AltaMateria.vue')

        },
        {
          path: '/Padres',
          name: 'Padres',
          component: () => import('../views/Padres de familia/padres_de_familia.vue')
        }

      ]
    },
  ]
})

// router.beforeEach((to, from, next) => {

//   const accessToken = sessionStorage.getItem('accessToken');
//   // Verificar si el usuario tiene un token 
//   if (to.name !== 'Login' && !accessToken) {
//     // Si el usuario intenta acceder a una ruta que no sea la de inicio de sesión y no tiene un token en las cookies,
//     next({ name: 'Login' });
//   } else {
//     // permitir el acceso a la ruta solicitada si tiene token 
//     next();
//   }
// });

export default router