import { useEffect, useState } from "react";
import { authApi } from "../../api/auth";
import { resetUser, setUser } from "../../redux/slices/authSlice";
import { useDispatch } from "react-redux";

export function useAuthMe(){

    const dispatch = useDispatch();
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchUser = async() => {
            try{
                setLoading(true);
                const response = await authApi.me();

                if(response.success){
                    dispatch(setUser({
                        email: response.data?.email
                    }));
                }
                else{
                    dispatch(resetUser());
                }
            }
            catch(e){
                dispatch(resetUser());
                dispatch(setError(e.message));
            }
            finally{
                setLoading(false);
            }
        }

        fetchUser();
    }, [dispatch]);

    return{
        loading
    };
}