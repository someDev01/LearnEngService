import { useDispatch, useSelector } from "react-redux";
import ButtonSignIn from "../../ui/button_signin/ButtonSignIn";
import HeroImg from "../../ui/hero_img/HeroImg";
import HeroText from "../../ui/hero_text/HeroText";
import styles from '../preview_hero/preview_hero.module.css';
import { useNavigate } from "react-router-dom";
import { openModalAuth } from "../../redux/slices/modalSlice";

function PreviewHero(){

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
        <section className={styles.preview_hero}>
            <HeroText isAuth={isAuth} onOpenModal={onOpenAuthModal}/>
            <HeroImg/>
        </section>
    )
}

export default PreviewHero;