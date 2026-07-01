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
import ButtonNavigate from '../../ui/button_navigate/ButtonNavigate';
import { Book, ListCheck, LogOut, LogOutIcon, LucideTvMinimalPlay, TvMinimalPlay } from 'lucide-react';
import Modal from '../../modal/Modal';
import { useEffect, useState } from 'react';
import ButtonX from '../../ui/button_x/ButtonX';
import Progress from '../progress/Progress';
import { noteApi } from '../../api/note';

function Menu({isOpen, onClose, email}){

    const dispatch = useDispatch();
    const navigation = useNavigate();

    const [isOpening, setIsOpening] = useState(false);
    const [notes, setNotes] = useState([]);

    useEffect(() => {
        if(isOpen){
            requestAnimationFrame(() => setIsOpening(true));
        }
        else{
            setIsOpening(false);
        }
    }, [isOpen]);

    useEffect(() => {
        const fetchDictionary = async() => {
            const response = await noteApi.getDictionary();

            if(response.success){
                setNotes(response.data);
            }
            else{
                setNotes([]);
                toast.error(response.error);
            }
        }

        fetchDictionary();
    }, []);

    const onNavigateDictionary = () => {
        navigation('/dictionary')
        dispatch(closeMenu());
    }
    const onNavigateVideos = () => {
        navigation('/videos')
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
        <Modal isOpen={isOpen}>
            <div className={`${styles.menu__container} ${isOpening ? styles.open : ''}`}>
                <div className={styles.menu_section}>
                    <div className={styles.header_part}>
                        <ButtonX onClick={onClose}/>
                        <Profile email={email}/>
                        <Progress userNotes={notes}/>
                    </div>
                    <div className={styles.middle_part}>
                        <div className={styles.menu_buttons}>
                            <ButtonNavigate 
                                onClick={onNavigateDictionary}
                                title="Личный словарь"
                                type="dict"
                                count={notes.length}
                            >
                                <Book size={18} color='#c4c4c4'/>
                            </ButtonNavigate>
                            <ButtonNavigate
                                onClick={() => {
                                    dispatch(openModalTraining());
                                }}
                                title="Тренировка"
                            >
                                <ListCheck size={20} color='#c4c4c4'/>
                            </ButtonNavigate>
                            <ButtonNavigate
                                onClick={onNavigateVideos}
                                title="Видео"
                            >
                                <TvMinimalPlay size={20} color='#c4c4c4'/>
                            </ButtonNavigate>
                        </div>
                    </div>
                    <div className={styles.bottom_part}>
                        <ButtonNavigate
                            onClick={onLogout}
                            title="Выйти"
                        >
                            <LogOut size={20} color='#f70000'/>
                        </ButtonNavigate>
                    </div>
                </div>
            </div>
        </Modal>
    )
}

export default Menu;