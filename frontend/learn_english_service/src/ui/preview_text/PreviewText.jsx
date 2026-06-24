import Opportunites from '../../widgets/opportunites/Opportunies';
import styles from '../preview_text/preview_text.module.css';


function PreviewText(){
    return(
        <>
            <div className={styles.block_text}>
                <div className={styles.text_preview}>
                    <p>Изучай англиский язык по коротким видео</p>
                </div>
            </div>
            <div className={styles.block_description}>
                <div className={styles.text_description}>
                    <p>Находи любимые сцены, кликай по словам в субтитрах и собирай свой словарь. Все в одном легком веб‑приложении</p>
                </div>
                <Opportunites/>
            </div>
        </>
    )
}

export default PreviewText;