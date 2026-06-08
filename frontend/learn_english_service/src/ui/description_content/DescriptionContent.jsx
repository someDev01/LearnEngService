import styles from '../description_content/description_content.module.css';

function DescriptionContent({name}){
    return(
        <>
            <div className={styles.block_description}>
                <p>{name}</p>
            </div>
        </>
    )
}

export default DescriptionContent;  