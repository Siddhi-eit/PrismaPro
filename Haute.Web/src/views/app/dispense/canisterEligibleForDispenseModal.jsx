import React from 'react';
import {
  Row,
  Card,
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
} from 'reactstrap';
import IntlMessages from 'helpers/IntlMessages';
import { Colxx } from 'components/common/CustomBootstrap';

const CanisterEligibleForDispenseModal = ({
  eligibleForDispenseMessage,
  isEligibleDispenseModalOpen,
  toggleModal,
}) => {
  return (
    <>
      {isEligibleDispenseModalOpen && (
        <Row>
          <Colxx xxs="12">
            <Card className="mb-4">
              <Modal isOpen={isEligibleDispenseModalOpen} toggle={toggleModal}>
                <ModalHeader>
                  <IntlMessages id="canisterEligibleForDispenseModal.title" />
                </ModalHeader>
                <ModalBody>{eligibleForDispenseMessage}</ModalBody>
                <ModalFooter>
                  <Button color="primary" onClick={toggleModal}>
                    Ok
                  </Button>{' '}
                </ModalFooter>
              </Modal>
            </Card>
          </Colxx>
        </Row>
      )}
    </>
  );
};

export default CanisterEligibleForDispenseModal;
