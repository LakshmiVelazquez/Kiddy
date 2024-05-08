<script>
import "@mdi/font/css/materialdesignicons.css";
import { useAuthStore } from '../../stores/auth'
import loginService from "../../services/login-service";
import { VForm } from "vuetify/components";
import { useToast } from 'vue-toastification'
import { nextTick } from 'vue';
import { storeToRefs } from "pinia";
export default {
  data() {
    return {
      toast: useToast(),
      showPassword: false,
      refVForm : '',
      datosUsuario:{
        userName:null,
        password:null
      }
    };
    
  },

  methods:{

    async login(){
      try{

        let res = await loginService.Login(this.datosUsuario);

        if(res.success){
          const {token, userName,tipoUsuario,userId}= res;
          this.$cookies.set('userName',userName)
          this.$cookies.set('accessToken', token);
          this.$cookies.set('tipoUsuario', tipoUsuario);
          this.$cookies.set('userId', userId);
          sessionStorage.setItem('accessToken', token)
          // Guardar el token y el tipo de usuario en el store
          // uthStore.setAuthData(token, userName, tipoUsuario);
          this.$router.push('/home');
        }else{
          this.toast.error(res.response.data.message)
        }

      }
      catch(error){
        console.log(error)
      }
    },

    async validarLogin(){
      
      if(this.datosUsuario.userName !=null && this.datosUsuario.password){
          
        this.datosUsuario.userName = this.datosUsuario.userName.trim();
        this.datosUsuario.password = this.datosUsuario.password.trim();
        this.login();
      }else{
        this.toast.warning("Ingresa tus credenciales")
      }
    }
  }
};
</script>

<template>
  <v-app>
    <v-main>
      <v-container fluid fill-height>
        <v-row align="center" justify="center" class="my-16">
          <v-col cols="12" lg="10">
            <v-card outlined class="rounded-lg">
              <VForm ref="refVForm" @submit.prevent="validarLogin()">
                <v-row class="">
                  <v-col cols="12" md="7" class="rounded-lg">
                    <div class="d-flex align-center justify-center">
                      <img
                        class="logo"
                        alt="Logo"
                        src="./../../assets/Logo.jpg"
                      />
                    </div>
                  </v-col>
                  <v-col cols="12" md="5" class="pa-8">
                    <v-card-title class="justify-center my-12">
                      <h1 class="display-1">Iniciar Sesión</h1>
                    </v-card-title>
                    <v-card-text>
                      <v-text-field
                        v-model="datosUsuario.userName"
                        label="Correo electrónico"
                        outlined
                        prepend-inner-icon="mdi-email me-1"
                        :rules="[requiredValidator]"
                      ></v-text-field>
                      <v-text-field
                        v-model="datosUsuario.password"
                        label="Contraseña"
                        outlined
                        :rules="[requiredValidator]"
                        prepend-inner-icon="mdi-lock me-1"
                        :type="showPassword ? 'text' : 'password'"
                        :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                        @click:append="showPassword = !showPassword"
                      ></v-text-field>
                    </v-card-text>
                    <v-card-text>
                      <div class="w-100 d-flex justify-end">
                        <VBtn type="submit" color="success">Ingresar</VBtn>
                      </div>
                    </v-card-text>
                  </v-col>
                </v-row>
              </VForm>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<style scoped>
.logo {
  width: 70%;
  height: 70%;
}
</style>
