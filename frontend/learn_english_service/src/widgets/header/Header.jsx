import { Link } from 'react-router-dom';
import styles from '../header/header.module.css';
import Logo from '../../ui/logo/Logo';
import ButtonMenu from '../../ui/button_menu/ButtonMenu';
import { useDispatch, useSelector } from 'react-redux';
import { closeMenu, openMenu } from '../../redux/slices/menuSlice';
import ButtonSignIn from '../../ui/button_signin/ButtonSignIn';
import AuthFlow from '../auth/AuthFlow';
import TrainingModal from '../training/TrainingModal';
import SideBar from '../side_bar/SideBar';

function Header(){

    const user = useSelector(state => state.auth.user);

    const isOpenMenu = useSelector(state => state.menu.isOpenMenu);
    const isOpenAuth = useSelector(state => state.modal.isOpenModalAuth);
    const isOpenTraining = useSelector(state => state.modal.isOpenModalTraining);
    const dispatch = useDispatch();

    const onOpen = () => {dispatch(openMenu());};
    const onClose = () => {dispatch(closeMenu());};

    return(
        <>
            <div className={styles.header__container}>
                <div className={styles.header_line}>
                    <Link to='/' style={{textDecoration: 'none'}} replace>
                        <Logo title="VoClip"/>
                    </Link>
                    {user && <ButtonMenu onOpen={onOpen}/>}
                </div>
            </div>
            {isOpenMenu && <SideBar isOpen={isOpenMenu} onClose={onClose}/>}
            {isOpenAuth && <AuthFlow isOpen={isOpenAuth}/>}
            {isOpenTraining && <TrainingModal isOpen={isOpenTraining}/>}
        </>
    )
}

export default Header;