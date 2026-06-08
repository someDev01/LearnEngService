import { useDispatch, useSelector } from 'react-redux';
import PreviewText from '../../ui/preview_text/PreviewText';
import styles from '../hero/hero.module.css';
import { openModalAuth } from '../../redux/slices/modalSlice';
import { useNavigate } from 'react-router-dom';
import GoToVideosButton from '../../ui/button_go_to_videos/GoToVideosButton';

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

    return(
        <div className={styles.hero}>
            <PreviewText/>
            <GoToVideosButton onClick={onToVideos}/>
        </div>
    )
}

export default Hero;