import ButtonSignIn from '../button_signin/ButtonSignIn';
import styles from '../hero_text/hero_text.module.css';

function HeroText({isAuth, onOpenModal}){
    return(
        <div className={styles.hero_text}>
            <header>
                <h3>Смотри видео.</h3>     
                <h3>Сохраняй заметки.</h3>
                <h3 style={{color:'#ff7300'}}>Закрепляй слова.</h3>
            </header>
            <div className={styles.hero_description}>
                <p>Собирай свой словарь английского из видео и закрепляй слова с помощью тренировки</p>
            </div>
            {!isAuth && <ButtonSignIn onClick={onOpenModal}/>}
        </div>
    )
}

export default HeroText;
