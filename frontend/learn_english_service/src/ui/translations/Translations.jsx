import styles from '../translations/translations.module.css';

function Translations({translations, size}){
    return(
        <div className={styles.translations} style={{fontSize: size}}>
            <p>{translations.join(', ')}</p>
        </div>
    )
}

export default Translations;