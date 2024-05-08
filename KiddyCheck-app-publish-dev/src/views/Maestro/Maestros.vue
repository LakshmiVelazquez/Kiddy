<script>
import { useToast } from "vue-toastification";
import gradoService from "../../services/grado-service";
import grupoService from "../../services/grupo/grupo-service";
import maestroService from "../../services/maestros/maestro-service";
import axios from "axios";
//import maestroService from '../../services/maestros/maestro-service'
import { VBtn, VDataTable } from "vuetify/components";

export default {
  components: {},
  data() {
    return {
      toast: useToast(),
      search: "",
      headers: [
        {
          align: "start",
          key: "nombreCompleto",
          sortable: false,
          title: "Nombre",
        },
        { key: "telefono", title: "Telefono" },
        { key: "correo", title: "Correo" },
        { key: "nombreGrado", title: "Grado" },
        { key: "nombreGrupo", title: "Grupo" },
        {
          title: "Acciones",
          key: "actions",
        },
      ],
      dialog: false,
      openEliminar: false,
      maestrosDatos: [],
      show2: true,
      password: "Contraseña",
      rules: {
        required: (value) => !!value || "Required.",
        min: (v) => v.length >= 8 || "Min 8 characters",
        emailMatch: () => `The email and password you entered don't match`,
      },
      requestMaestros: {
        nombre: "",
        apellidoPaterno: "",
        apellidoMaterno: "",
        telefono: "",
        grupo: null,
        correo: "",
        password: "",
        idTipoPersona: 2,
        id: 0,
      },
      requestMaestrosBase: {
        nombre: "",
        apellidoPaterno: "",
        apellidoMaterno: "",
        telefono: "",
        grupo: null,
        correo: "",
        password: "",
        idTipoPersona: 2,
        id: 0,
      },
      idMaestro: null,
      grados: [],
      grupos: [],
    };
  },
  methods: {
    async obtenerMaestros() {
      try {
        const response = await maestroService.obtenerPersonasPorTipo(2);

        this.maestrosDatos = response.data;
      } catch (error) {
        // Manejar errores aquí
        console.error("Error al obtener maestros:", error);
      }
    },

    async GetGrados() {
      try {
        this.grados = await gradoService.ObtenerGrados();
      } catch (error) {
        console.error(error);
      }
    },
    async GetGrupos() {
      try {
        this.grupos = await grupoService.ObtenerGrupo();
      } catch (error) {
        console.error(error);
      }
    },
    async abrirGuardar() {
      this.requestMaestros = { ...this.requestMaestrosBase };
      this.dialog = true;
    },

    async cancelar() {
      this.requestMaestros = { ...this.requestMaestrosBase };
      this.dialog = false;
    },

    guardarMaestro() {
      console.log(this.requestMaestros);
      this.dialog = false;
      this.agregarMaestro();
    },

    async agregarMaestro() {
      if (this.requestMaestros.id == 0) {
        let response = await maestroService.agregarPersona(this.requestMaestros);

        if (response.status == 200) {
          this.toast.success("Maestro agregado");
          await this.obtenerMaestros();
        } else {
          this.toast.success("Ocurrio un error");
        }
      } else {
        let response = await maestroService.editarPersona(this.requestMaestros);
        if (response.status == 200) {
          this.toast.success("Maestro actualizado");
          await this.obtenerMaestros();
        } else {
          this.toast.success("Ocurrio un error");
        }
      }
    },

    eliminar(item) {
      this.openEliminar = true;
      this.idMaestro = item.id;
    },
    cerrarEliminar() {
      this.openEliminar = false;
    },

    async confirmarEliminar() {
      let response = await maestroService.eliminarPersona(this.idMaestro);
      if (response.data.success == true) {
        this.toast.success("Maestro eliminado");
        this.openEliminar = false;
        await this.obtenerMaestros();
      }
    },

    editar(item) {
      this.requestMaestros = { ...item };
      this.dialog = true;
    },

    cancelar() {
      this.requestMaestros = { ...this.requestMaestrosBase };
      this.dialog = false;
    },
  },

  async created() {
    await this.obtenerMaestros();
    await this.GetGrados();
    await this.GetGrupos();
  },
};
</script>
<template>
  <v-container>
    <v-row class="justify-center">
      <v-col cols="12" sm="11" md="10">
        <v-row>
          <v-col cols="12" sm="5">
            <div class="d-flex">
              <p class="text-h4">
                <v-icon color="green" size="48"> mdi mdi-human-male-board </v-icon>
                Docentes
              </p>
              <v-btn @click="abrirGuardar()" icon="mdi-plus" size="40" class="ms-4">
              </v-btn>
            </div>
          </v-col>
          <v-col cols="12" sm="7">
            <v-text-field variant="outlined" v-model="search" label="Buscar">
              <template v-slot:prepend>
                <v-icon size="40" color="green">mdi mdi-magnify </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
      </v-col>

      <v-col cols="12" sm="11" md="10">
        <v-card>
          <template v-slot:title>
            <span class="font-weight-black">Datos de los Docentes</span>
          </template>
          <div class="py-5 border-top">
            <v-data-table :headers="headers" :items="maestrosDatos" :search="search">
              <template #item.actions="{ item, index }">
                <div class="d-flex gap-1">
                  <VBtn
                    @click="editar(item)"
                    size="35"
                    color="success"
                    icon="mdi-pencil"
                    class="me-1"
                  >
                  </VBtn>
                  <VBtn @click="eliminar(item)" size="35" color="error" icon="mdi-delete">
                  </VBtn>
                </div>
              </template>
            </v-data-table>
          </div>
        </v-card>
      </v-col>
    </v-row>
    <v-dialog v-model="dialog" max-width="700">
      <v-card :title="requestMaestros.id != 0 ? 'Editar Maestro' : 'guardar Maestro'">
        <v-card-text class="px-4">
          <v-form>
            <v-container>
              <v-row>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="requestMaestros.nombre"
                    label="Nombre"
                  ></v-text-field>
                </v-col>

                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="requestMaestros.apellidoPaterno"
                    label="Apellido paterno"
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="requestMaestros.apellidoMaterno"
                    label="Apellido materno"
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="requestMaestros.telefono"
                    label="Telefono"
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-select
                    v-model="requestMaestros.grado"
                    label="Grado"
                    :items="grados"
                    item-title="grado"
                    item-value="id"
                  ></v-select>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-select
                    v-model="requestMaestros.grupo"
                    label="Grupo"
                    :items="grupos"
                    item-title="nombre"
                    item-value="id"
                  ></v-select>
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="requestMaestros.correo"
                    label="Correo"
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    :append-icon="show2 ? 'mdi-eye' : 'mdi-eye-off'"
                    :rules="[rules.required, rules.min]"
                    :type="show2 ? 'text' : 'password'"
                    class="input-group--focused"
                    hint="At least 8 characters"
                    label="Contraseña"
                    name="input-10-2"
                    v-model="requestMaestros.password"
                    @click:append="show2 = !show2"
                  ></v-text-field>
                </v-col>
              </v-row>
            </v-container>
          </v-form>
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn text="Cancelar" @click="cancelar()"></v-btn>
          <v-btn
            color="surface-variant"
            text="Guardar"
            variant="flat"
            @click="guardarMaestro()"
          ></v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <VDialog v-model="openEliminar" max-width="500px">
      <VCard>
        <VCardTitle class="d-flex justify-center mt-1 mb-1"
          >¿Deseas eliminar el registro?
        </VCardTitle>
        <VCardActions>
          <VSpacer />
          <VBtn color="error" variant="outlined" @click="CerrarEliminar"> Cancelar </VBtn>

          <VBtn color="success" variant="elevated" @click="confirmarEliminar">
            Aceptar
          </VBtn>
          <VSpacer />
        </VCardActions>
      </VCard>
    </VDialog>
  </v-container>
</template>
<style lang="scss" scoped></style>
