import { $apiService } from '../utils/api-services';

export default {

    Login: async (data) => {
        try {
            var res = await $apiService.post('/api/v1/Auth/Login', data).then(data => data)
            return res.data;
        } catch (error) {
            return error;
        }
    }
}