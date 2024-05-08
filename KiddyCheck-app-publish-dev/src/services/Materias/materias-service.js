import { $apiService } from '../../utils/api-services';

export default {

    Materias: async () => {
        try {
            var res = await $apiService.get('/api/v1/Materias/Listar').then(data => data)
            return res.data;
        } catch (error) {
            return error;
        }
    },

    Agregar: async (data) => {
        try {
            var res = await $apiService.post('/api/v1/Materias/Agregar', data).then(data => data)
            return res;
        } catch (error) {
            return error;
        }
    },

    Actualizar: async (data) => {
        try {
            var res = await $apiService.put('/api/v1/Materias/Editar', data).then(data => data)
            return res;
        } catch (error) {
            return error;
        }
    },
    Eliminar: async (id) => {
        try {
            var res = await $apiService.delete('/api/v1/Materias/Eliminar?id=' + id).then(data => data)
            return res;
        } catch (error) {
            return error;
        }
    },
    ObtenerXId: async (id) => {
        try {
            var res = await $apiService.get('/api/v1/Materias/ObtenerXId?id=' + id).then(data => data)
            return res.data;
        } catch (error) {
            return error;
        }
    }
};