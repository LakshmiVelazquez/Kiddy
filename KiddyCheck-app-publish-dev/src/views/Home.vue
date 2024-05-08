
<template>
  <v-container>
    <v-row>
      <v-col cols="12" sm="12">
        <div class="text-titulo">
          <h1>Bienvenidos</h1>
        </div>
      </v-col>
      <v-col cols="12" sm="6">
        <div class="border rounded-1 px-5 py-5 shadow-lg">
          <div class="conte">
            <div class="text-center">
              <p color="blue-grey-darken-3" class="text-h5 font-weight-light">
                Asistencias diarias
              </p>
              <p class="text-h3 mx-auto mt-5">{{contadorDia}}</p>
            </div>
            <div>
              <img width="150" alt="Logo" src="./../assets/Logo.jpg" />
            </div>
          </div>
        </div>
      </v-col>
      <v-col cols="12" sm="6">
        <div class="border rounded-1 px-5 py-5 shadow-lg">
          <div class="conte">
            <div class="text-center">
              <p color="blue-grey-darken-3" class="text-h5 font-weight-light">
                Asistencias Semanales
              </p>
              <p class="text-h3 mx-auto mt-5">{{contadorSemanal}}</p>
            </div>
            <div>
              <img width="150" alt="Logo" src="./../assets/Logo.jpg" />
            </div>
          </div>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>
<script>
  import axios from "axios";
 export default {
  data() {
    return {
      contadorDia: 0,
      contadorSemanal: 0
    }
  },
  methods: {
    async AsistenciasDia() {
      let result = await axios.get('https://localhost:7163/api/v1/Alumno/AsistenciasHoy')
      this.contadorDia = result.data
    },
    async AsistenciasSemanales() {
      let result = await axios.get('https://localhost:7163/api/v1/Alumno/AsistenciasSemanal')
      this.contadorSemanal = result.data
    }
  },
  async created() {
    await this.AsistenciasDia()
    await this.AsistenciasSemanales()
  }
 }
</script>
<style>
.text-titulo {
  font-family: "Courier New", Courier, monospace;
  font: bolder;
  text-align: center;
  font-size: 50px;
}

.text-titulo h1 {
  background: linear-gradient(90deg, #026e2bfd, #40ff00);
  -webkit-background-clip: text;
  background-clip: text;
  -webkit-text-fill-color: transparent;
}
.border {
  box-shadow: 0 2px 16px hsla(0, 0%, 0%, 0.08),
    0 0.25px 2px hsla(0, 0%, 0%, 0.16), inset 0 2px 16px transparent,
    inset 0 0.25px 2px transparent;
  border-radius: 15px;
}

.conte {
  display: flex;
  justify-content: space-between;
}
</style>
