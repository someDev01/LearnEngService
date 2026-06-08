import { useDispatch } from 'react-redux';
import { authApi } from '../../api/auth';
import ButtonDictionary from '../../ui/button_dictionary/ButtonDictionary';
import ButtonSignOut from '../../ui/button_signout/ButtonSignOut';
import Profile from '../../ui/profile/Profile';
import styles from '../menu/menu.module.css';
import { resetStep, resetUser, setError } from '../../redux/slices/authSlice';
import { toast } from 'react-toastify';
import { closeMenu } from '../../redux/slices/menuSlice';
import { useNavigate } from 'react-router-dom';
import ButtonTraining from '../../ui/button_training/ButtonTraining';
import { openModalTraining } from '../../redux/slices/modalSlice';

function Menu({isOpen, email}){

    const dispatch = useDispatch();
    const navigation = useNavigate();

    if(!isOpen) return null;

    const onNavigate = () => {
        navigation('/dictionary')
        dispatch(closeMenu());
    }

    const onLogout = async() => {
        try{
            const response = await authApi.logout();
            if(response.success){
                dispatch(resetStep());
                dispatch(resetUser());
                toast.success('Вы вышли из аккаунта');
            }
            else{
                dispatch(setError(response.error));
                toast.error(response.error);
            }
        }
        catch(e){
            toast.error(e.error.message);
            e.response?.data?.message || e.message;
        }
        finally{
            dispatch(closeMenu());
            navigation('/');
        }
    }

    return(
        <div className={styles.menu__container}>
            <div className={styles.menu_section}>
                <Profile email={email}/>
                <div className={styles.line_dividing}></div>
                <ButtonDictionary onClick={onNavigate}/>
                <ButtonTraining onClick={() => {
                    dispatch(openModalTraining());
                    dispatch(closeMenu())
                }}/>
                <ButtonSignOut onClick={onLogout}/>
            </div>
        </div>
    )
}

export default Menu;