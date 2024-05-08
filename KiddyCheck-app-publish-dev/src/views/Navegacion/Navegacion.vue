<template>
  <v-card>
    <v-layout>
      <v-app-bar color="white" prominent>
        <div class="ms-2">
          <RouterLink to="/Home">
            <img class="logo" alt="Logo" src="./../../assets/Logo.jpg" />
          </RouterLink>
        </div>
        <v-card-title class="px-1">
          <RouterLink to="/Home" class="titulo text-decoration-none">
            <p style="color: #263238">KIDDYCHECK</p>
          </RouterLink>
        </v-card-title>

        <v-app-bar-nav-icon
          variant="text"
          color="success"
          @click.stop="drawer = !drawer"
        ></v-app-bar-nav-icon>

        <v-spacer></v-spacer>

        <!-- <template v-if="$vuetify.display.mdAndUp">
          <v-btn icon="mdi-magnify" color="success" variant="text"></v-btn>

          <v-btn icon="mdi-filter" variant="text"></v-btn>
        </template> -->

        <v-menu transition="scale-transition">
          <template v-slot:activator="{ props }">
            <v-icon
              class="me-4"
              color="success"
              icon="mdi2 mdi mdi-account-circle-outline"
              v-bind="props"
            ></v-icon>
          </template>

          <v-list class="btn-salir">
            <p @click="logout()" class="mx-9 py-0">Salir</p>
          </v-list>
        </v-menu>
      </v-app-bar>

      <v-navigation-drawer
        v-model="drawer"
        :location="$vuetify.display.mobile ? 'bottom' : undefined"
        temporary
      >
        <!-- <RouterLink to="/">Hola2 </RouterLink>
        <RouterLink to="/usuarios">Hola3 </RouterLink> -->
        <div class="py-2 px-5 my-4">
          <p class="text-body-2 title-menu">MENU</p>
        </div>
        <div class="conte-menu">
          <RouterLink class="btn-menu py-2" to="/Grado">
            <div class="conte-submenu">
              <v-icon
                class="mx-2"
                icon="mdi mdi-circle-multiple-outline"
              ></v-icon>
              <p class="">Grados</p>
            </div>
          </RouterLink>
          <RouterLink class="btn-menu py-2" to="/Grupo">
            <div class="conte-submenu">
              <v-icon class="mx-2" icon="mdi mdi-account-group"></v-icon>
              <p class="">Grupos</p>
            </div>
          </RouterLink>
          <RouterLink class="btn-menu py-2" to="/alumnos">
            <div class="conte-submenu">
              <v-icon class="mx-2" icon="mdi mdi-account-box"></v-icon>
              <p class="">Alumnos</p>
            </div>
          </RouterLink>
          <RouterLink class="btn-menu py-2" to="/Padres">
            <div class="conte-submenu">
              <v-icon class="mx-2" icon="mdi mdi-human-male-boy"></v-icon>
              <p class="">Tutor</p>
            </div>
          </RouterLink>
          <RouterLink
            v-if="this.$cookies.get('tipoUsuario') != 2"
            class="btn-menu py-2"
            to="/Maestros"
          >
            <div class="conte-submenu">
              <v-icon class="mx-2" icon="mdi mdi-human-male-board"></v-icon>
              <p class="">Docente</p>
            </div>
          </RouterLink>
          <RouterLink class="btn-menu py-2" to="/AltasMaterias">
            <div class="conte-submenu">
              <v-icon class="mx-2" icon="mdi mdi-text-long"></v-icon>
              <p class="">Materia</p>
            </div>
          </RouterLink>
        </div>
      </v-navigation-drawer>

      <v-main>
        <RouterView />
      </v-main>
    </v-layout>
  </v-card>
</template>

<script>
import { RouterLink } from "vue-router";

export default {
  components: {
    RouterLink,
  },
  data: () => ({
    drawer: false,
    group: null,
    tipoUsuario: null,
    items: [
      {
        title: "Foo",
        value: "foo",
      },
      {
        title: "Bar",
        value: "bar",
      },
      {
        title: "Fizz",
        value: "fizz",
      },
      {
        title: "Buzz",
        value: "buzz",
      },
    ],
  }),

  methods: {
    logout() {
      console.log("salir");
      this.$cookies.remove("userName");
      this.$cookies.remove("accessToken");
      this.$cookies.remove("tipoUsuario");
      sessionStorage.removeItem("accessToken");

      this.$router.push("/");
    },
  },

  watch: {
    group() {
      this.drawer = false;
    },
  },

  async created() {},
};
</script>

<style scoped>
.titulo {
  width: 150px !important;
}

.title-menu {
  color: #9ca3af;
}
.btn-conte {
  background-color: black;
  color: white;
}

.btn-salir:hover {
  cursor: pointer;
  background-color: #4caf50;
  color: white;
}

.conte-menu {
  display: grid;
}

.btn-menu {
  width: auto;
  padding: 10px;
  border-bottom: 1px #eeeeee solid;
  text-decoration: none;
  color: #52525b;
  font: medium;
}

.btn-menu:hover {
  padding: 12px;
  font-size: large;
  background-color: #4caf50;
  color: white;
  text-decoration: none;
}

.conte-submenu {
  display: flex;
}

.logo {
  width: 40px;
  height: 40px;
}

.mdi2 {
  width: 40px;
  height: 40px;
}
</style>
