<script>
import { VContainer, VRow } from "vuetify/components";
import materiasService from "../../services/Materias/materias-service";
import gradoService from "../../services/grado-service";
import { useToast } from "vue-toastification";

export default {
  components: {},
  data() {
    return {
      /* Variables */
      toast: useToast(),
      OpenAgregar: false,
      modalEliminar: false,
      refFormulario: "",
      idEliminar: 0,

      materiaDatos: {
        id: 0,
        nombre: "",
        grados: [],
      },

      materiaBase: {
        id: 0,
        nombre: "",
        grados: [],
      },

      Materias: [],
      headers: [
        { key: "id", title: "#" },
        { key: "nombre", title: "Nombre" },
        { key: "gradosNombres", title: "Grados" },
        { key: "acciones", title: "Opciones" },
      ],
      /* <<---------->> */
      Grados: [],

      search: "",
    };
  },
  methods: {
    /* <<<<<------------  METODOS CRUD  ------------->>>> */

    async _Grados() {
      try {
        let res = await gradoService.ObtenerGrados().then((data) => data);
        if (res) this.Grados = res;
      } catch (error) {
        return error;
      }
    },

    async _Materias() {
      try {
        var res = await materiasService.Materias().then((data) => data);

        if (res?.data) {

          res.data.map((data) => {
              let datos = ''
              
              data.gradosNombres.map((d, k) => {
              datos += data.gradosNombres.length == k + 1 ? d : d + ", ";
              //return data.gradosNombres.length == k + 1 ? d : d + ", ";
            });
            data.gradosNombres = datos
          });
          this.Materias = res.data;
        }
      } catch (error) {
        console.log(error);
      }
    },

    async _Agregar(data) {
      try {
        var res = await materiasService.Agregar(data).then((data) => data);
        return res;
      } catch (error) {
        return error;
      }
    },

    async _Eliminar() {
      try {
        var res = await materiasService.Eliminar(this.idEliminar).then((data) => data);
        return res;
      } catch (error) {
        return error;
      }
    },

    async _Actualizar(data) {
      try {
        var res = await materiasService.Actualizar(data).then((data) => data);
        return res;
      } catch (error) {
        return error;
      }
    },

    async _ObtenerXId() {
      try {
        var res = await materiasService.ObtenerXId().then((data) => data);
        return res;
      } catch (error) {
        return error;
      }
    },

    /* <<<<<------------------------->>>> */

    /* <<<<<------------  METODOS MODALES  ------------->>>> */
    async _abrirAgregar() {
      this.materiaDatos = { ...this.materiaBase };
      this.OpenAgregar = true;
    },
    async _cerrarAgregar() {
      this.materiaDatos = { ...this.materiaBase };
      this.OpenAgregar = false;
    },

    async _abrirEditar(item) {
      this.materiaDatos = { ...item };
      this.OpenAgregar = true;
    },
    async _cerrarEditar() {
      this.materiaDatos = { ...this.materiaBase };
      this.OpenAgregar = false;
    },

    async _abrirEliminar(id) {
      this.idEliminar = id;
      this.modalEliminar = true;
    },
    async _cerrarEliminar() {
      this.modalEliminar = false;
    },
    /* <<<<<------------------------->>>> */

    /* <<<<<------------  METODOS VISTA  ------------->>>> */
    async onsubmit() {
      let form = await this.$refs.refFormulario.validate();
      if (form.valid) {
        this.Guardar();
      }
    },

    async Guardar() {
      if (this.materiaDatos.id == 0) {
        let result = await this._Agregar(this.materiaDatos).then((data) => data);
        if (result?.data?.success) {
          this.toast.success(result.data.message);
          this._cerrarAgregar();
          await this._Materias();
        } else {
          this.toast.error(result.response.data.message);
          this._cerrarAgregar();
        }
      } else {
        let result = await this._Actualizar(this.materiaDatos).then((data) => data);
        if (result?.data?.success) {
          this.toast.success(result.data.message);
          this._cerrarEditar();
          await this._Materias();
        } else {
          this.toast.error(result.response.data.message);
          this._cerrarEditar();
        }
      }
    },

    async _EliminarRegistro() {
      try {
        let result = await this._Eliminar().then((data) => data);

        if (result?.data?.success) {
          this.toast.success(result.data.message);
          this._cerrarEliminar();
          await this._Materias();
        } else {
          this.toast.error(result.response.data.message);
          this._cerrarEliminar();
        }
      } catch (error) {
        console.log(error);
      }
    },
    /* <<<<<------------------------->>>> */
  },
  async created() {
    // this.loadItems({ page: 1, itemsPerPage: this.itemsPerPage, sortBy: 'desc' })
    await this._Materias();
    await this._Grados();
  },
};
</script>

<template>
  <div>
    <VContainer>
      <VRow class="mb-2">
        <v-col cols="12" md="11">
          <v-row>
            <v-col cols="12" sm="5">
              <div class="d-flex">
                <p class="text-h4">
                  <v-icon color="green" size="48"> mdi mdi-human-male-board </v-icon>
                  Materias
                </p>
                <VBtn
                  class="ms-2"
                  color="cafe"
                  @click="_abrirAgregar"
                  size="40"
                  rounded="pill"
                  icon="mdi-plus"
                >
                </VBtn>
              </div>
            </v-col>
            <v-col cols="12" sm="7">
              <v-text-field
                v-model="search"
                label="Search"
                prepend-inner-icon="mdi-magnify"
                variant="outlined"
                hide-details
                single-line
              ></v-text-field>
            </v-col>
          </v-row>
        </v-col>
      </VRow>
      <v-card flat>
        <div class="py-5 border-top">
          <v-data-table :headers="headers" :items="Materias" :search="search">
            <template #item.acciones="{ item }">
              <div class="d-flex gap-2">
                <VBtn
                  size="35"
                  title="Editar"
                  icon="mdi-pencil"
                  color="success"
                  @click="_abrirEditar(item)"
                >
                </VBtn>
                <VBtn
                  size="35"
                  class="ml-2"
                  title="Eliminar"
                  icon="mdi-delete"
                  color="error"
                  @click="_abrirEliminar(item.id)"
                >
                </VBtn>
              </div>
            </template>
          </v-data-table>
        </div>
      </v-card>
    </VContainer>
  </div>

  <VDialog v-model="OpenAgregar" max-width="500px" persistent>
    <VCard style="overflow: hidden">
      <VCardTitle class="pa-6">
        <span class="headline">{{
          materiaDatos.id != 0 ? "Editar materia" : "Agregar materia"
        }}</span>
      </VCardTitle>
      <VCardText class="pt-0 pb-0">
        <VForm ref="refFormulario" @submit.prevent="() => onsubmit()">
          <VRow>
            <v-col cols="12" md="12">
              <v-text-field
                v-model="materiaDatos.nombre"
                label="Materia"
                validate-on="input"
              />
            </v-col>

            <v-col>
              <v-select
                color="#b48d57"
                v-model="materiaDatos.grados"
                :items="Grados"
                item-title="grado"
                item-value="id"
                label="Seleccione los grados correspondientes"
                density="compact"
                placeholder="Seleccione grados"
                multiple
              />
              <!-- <v-select v-model="materiaDatos.grados" :items="Grados" label="Chips" chips
                                multiple></v-select> -->
            </v-col>
            <v-col cols="12">
              <VCardActions>
                <VSpacer />
                <VBtn color="error" variant="outlined" @click="_cerrarAgregar">
                  Cerrar
                </VBtn>

                <VBtn color="success" variant="elevated" type="submit"> Guardar </VBtn>
              </VCardActions>
            </v-col>
          </VRow>
        </VForm>
      </VCardText>
    </VCard>
  </VDialog>
  <VDialog v-model="modalEliminar" max-width="500px" persistent>
    <VCard>
      <VCardTitle class="d-flex justify-center mt-1 mb-1">
        ¿Deseas eliminar el registro?
      </VCardTitle>
      <VCardActions>
        <VSpacer />
        <VBtn color="error" variant="outlined" @click="_cerrarEliminar"> Cancelar </VBtn>

        <VBtn color="success" variant="elevated" @click="_EliminarRegistro">
          Aceptar
        </VBtn>
        <VSpacer />
      </VCardActions>
    </VCard>
  </VDialog>
</template>

<style lang="scss" scoped></style>
