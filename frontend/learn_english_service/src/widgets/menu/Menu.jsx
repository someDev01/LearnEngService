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
import { Book, Ear, ListCheck, LogOut, LogOutIcon, LucideTvMinimalPlay, TvMinimalPlay } from 'lucide-react';
import Modal from '../../modal/Modal';
import { useEffect, useState } from 'react';
import ButtonX from '../../ui/button_x/ButtonX';
import Progress from '../progress/Progress';
import { profileApi } from '../../api/profile';

function Menu({isOpen, onClose, user}){

    const dispatch = useDispatch();
    const navigation = useNavigate();

    const [isOpening, setIsOpening] = useState(false);
    const [profileData, setProfileData] = useState(null);

    useEffect(() => {
        if(isOpen){
            requestAnimationFrame(() => setIsOpening(true));
        }
        else{
            setIsOpening(false);
        }
    }, [isOpen]);

    useEffect(() => {
        const fetchProfile = async() => {
            const response = await profileApi.getProfile();

            if(response.success){
                setProfileData(response.data);
            }
            else{
                setProfileData(null);
                toast.error(response.error);
            }
        }

        fetchProfile();
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
                        <Profile user={user}/>
                        <Progress addedCount={profileData.addedCount} trainedCount={profileData.trainedCount}/>
                    </div>
                    <div className={styles.middle_part}>
                        <div className={styles.menu_buttons}>
                            <ButtonNavigate 
                                onClick={onNavigateDictionary}
                                title="Личный словарь"
                                type="dict"
                                count={profileData.notesCount}
                            >
                                <Book size={18} color='rgb(255, 86, 13)'/>
                            </ButtonNavigate>
                            <ButtonNavigate
                                onClick={() => {
                                    dispatch(openModalTraining());
                                }}
                                title="Тренировка"
                                type="train"
                            >
                                <Ear size={18} color='rgb(255, 223, 13)'/>
                            </ButtonNavigate>
                            <ButtonNavigate
                                onClick={onNavigateVideos}
                                title="Видео"
                                type="videos"
                                count={profileData.videosCount}
                            >
                                <TvMinimalPlay size={18} color='rgb(255, 86, 13)'/>
                            </ButtonNavigate>
                        </div>
                    </div>
                    <div className={styles.bottom_part}>
                        <ButtonNavigate
                            onClick={onLogout}
                            title="Выйти"
                            type="logout"
                        >
                            <LogOut size={18} color='#f70000'/>
                        </ButtonNavigate>
                    </div>
                </div>
            </div>
        </Modal>
    )
}

export default Menu;