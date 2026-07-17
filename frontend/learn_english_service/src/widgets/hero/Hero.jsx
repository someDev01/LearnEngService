import { useDispatch, useSelector } from 'react-redux';
import PreviewText from '../../ui/preview_text/PreviewText';
import styles from '../hero/hero.module.css';
import { openModalAuth } from '../../redux/slices/modalSlice';
import { useNavigate } from 'react-router-dom';
import Opportunites from '../opportunites/Opportunies';
import ButtonSignIn from '../../ui/button_signin/ButtonSignIn';

function Hero(){

    const user = useSelector(state => state.auth.user);
    const dispatch = useDispatch();
    const isAuth = !!user;

    const navigation = useNavigate();

    const onToVideos = () => {
        if(!isAuth){
            dispatch(openModalAuth());
            return;
        }

        navigation('/videos', {replace:true});
    };

    const onOpenAuthModal = () => {
        dispatch(openModalAuth());
    };

    return(
        <div className={styles.hero}>
            <PreviewText/>
            <ButtonSignIn onClick={onOpenAuthModal}/>
            <Opportunites/>
        </div>
    )
}

export default Hero;